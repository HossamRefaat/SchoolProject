using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
    #endregion

    #region Constructor
    public AuthenticationService(JwtSettings jwtSettings,
                                 IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtSettings = jwtSettings;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokens = new ConcurrentDictionary<string, RefreshTokenResult>();
    }
    #endregion

    #region Methods
    public async Task<JwtAuthRsult> GetJWTToken(User user)
    {
        var claims = new[]
        {
            new Claim(nameof(UserClaimModel.UserName), user.UserName),
            new Claim(nameof(UserClaimModel.Email), user.Email),
            new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
        };

        var accessToken = new JwtSecurityToken( issuer: _jwtSettings.Issuer,
                                                audience: _jwtSettings.Audience,
                                                claims: claims,
                                                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                                                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret )), SecurityAlgorithms.HmacSha256Signature));
        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var refreshToken = GetRefreshToken(user.UserName);
        var userRefreshToken = new UserRefreshToken
        {
            AddedTime = DateTime.Now,
            ExpiryDate = DateTime.Now.AddMonths(_jwtSettings.RefreshTokenExpireDate),
            isUsed = false,
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
            ExpireAt = DateTime.Now.AddMonths(_jwtSettings.RefreshTokenExpireDate)
        };

        return _refreshTokens.AddOrUpdate(refreshTokenResult.RefreshToken,refreshTokenResult, (key, oldValue) => refreshTokenResult);
    }
    #endregion
}
