using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service;

public class AuthenticationService : IAuthenticationService
{
    #region Fields
    private readonly JwtSettings _jwtSettings;
    private readonly ConcurrentDictionary<string, RefreshTokenResult> _refreshTokens;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly UserManager<User> _userManager;
    #endregion

    #region Constructor
    public AuthenticationService(JwtSettings jwtSettings,
                                 IRefreshTokenRepository refreshTokenRepository,
                                 UserManager<User> userManager)
    {
        _jwtSettings = jwtSettings;
        _refreshTokenRepository = refreshTokenRepository;
        _userManager = userManager;
        _refreshTokens = new ConcurrentDictionary<string, RefreshTokenResult>();
    }
    #endregion

    #region Methods
    public async Task<JwtAuthRsult> GetJWTToken(User user)
    {
        var (accessToken, accessTokenString) = GenerateJWTToken(user);
        var refreshToken = GetRefreshToken(user.UserName);
        var userRefreshToken = new UserRefreshToken
        {
            AddedTime = DateTime.Now,
            ExpiryDate = DateTime.Now.AddMonths(_jwtSettings.RefreshTokenExpireDate),
            isUsed = true,
            IsRevoked = false,
            JwtId = accessToken.Id,
            Token = accessTokenString,
            RefreshToken = refreshToken.RefreshToken,
            UserId = user.Id
        };

        await _refreshTokenRepository.AddAsync(userRefreshToken);

        return new JwtAuthRsult
        {
            AccessToken = accessTokenString,
            refreshTokenResult = refreshToken
        };
    }

    
    public async Task<JwtAuthRsult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expireDate, string refreshToken)
    {
        
        var (jwtSecurityToken, accessTokenString) = GenerateJWTToken(user);

        var response = new JwtAuthRsult();
        response.AccessToken =  accessTokenString;
        var refreshTokenResult = new RefreshTokenResult
        {
            UserName = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.UserName))?.Value,
            RefreshToken = refreshToken,
            ExpireAt = (DateTime)expireDate
        };
        response.refreshTokenResult = refreshTokenResult;
        return response;
    }

    public async Task<string> ValidateToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var response = tokenHandler.ReadJwtToken(accessToken);
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = _jwtSettings.ValidateIssuer,
            ValidIssuers = new []{_jwtSettings.Issuer},
            ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
            ValidAudience = _jwtSettings.Audience,
            ValidateAudience = _jwtSettings.ValidateAudience,
            ValidateLifetime = _jwtSettings.ValidateLifeTime,
        };
        try
        {
            var validator = tokenHandler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);
            if(validator == null)
                throw new SecurityTokenException("Invalid token");

            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private (JwtSecurityToken, string) GenerateJWTToken(User user)
    {
        var claims = new[]
        {
            new Claim(nameof(UserClaimModel.UserName), user.UserName),
            new Claim(nameof(UserClaimModel.Email), user.Email),
            new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
            new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
        };

        var accessToken = new JwtSecurityToken( issuer: _jwtSettings.Issuer,
                                                audience: _jwtSettings.Audience,
                                                claims: claims,
                                                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                                                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret )), SecurityAlgorithms.HmacSha256Signature));
        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
        return (accessToken, accessTokenString);
    }


    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];    
        var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private RefreshTokenResult GetRefreshToken(string UserName)
    {
       var refreshTokenResult = new RefreshTokenResult
        {
            UserName = UserName,
            RefreshToken = GenerateRefreshToken(),
            ExpireAt = DateTime.Now.AddHours(_jwtSettings.RefreshTokenExpireDate)
        };

        return _refreshTokens.AddOrUpdate(refreshTokenResult.RefreshToken,refreshTokenResult, (key, oldValue) => refreshTokenResult);
    }

    public JwtSecurityToken ReadJWTToken(string accessToken)
    {
        if(string.IsNullOrEmpty(accessToken))
            throw new ArgumentException("Access token is null or empty.", nameof(accessToken));
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var response = tokenHandler.ReadJwtToken(accessToken);
       
        return response;
    }

    public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
    {
        if(jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            return ("Algorithm is not valid", null);
        if(jwtToken.ValidTo>DateTime.UtcNow)
            return ("Access token is not expired yet", null);

        var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id))?.Value;
        var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                                              .FirstOrDefaultAsync(x=>x.Token == accessToken &&
                                                                   x.RefreshToken == refreshToken &&
                                                                   x.Id == int.Parse(userId));

        if(userRefreshToken == null)
            return ("Refresh token does not exist",null);

        if(userRefreshToken.ExpiryDate < DateTime.UtcNow)
        {
            userRefreshToken.IsRevoked = true;
            userRefreshToken.isUsed = false;
            await _refreshTokenRepository.UpdateAsync(userRefreshToken);
            return ("Refresh token is expired", null);
        }
        return (userId, userRefreshToken.ExpiryDate);
    }
    #endregion
}
