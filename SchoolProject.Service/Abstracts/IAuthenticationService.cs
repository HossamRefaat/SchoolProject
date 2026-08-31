using System.IdentityModel.Tokens.Jwt;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;

namespace SchoolProject.Service.Abstracts;

public interface IAuthenticationService
{
    public Task<JwtAuthRsult> GetJWTToken(User user);
    public JwtSecurityToken ReadJWTToken(string accessToken);
    public Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
    public Task<JwtAuthRsult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expireDate, string refreshToken);
    public Task<string> ValidateToken(string accessToken);
    
}
