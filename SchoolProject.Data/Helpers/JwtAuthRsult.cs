namespace SchoolProject.Data.Helpers;

public class JwtAuthRsult
{
    public string AccessToken { get; set; }
    public RefreshTokenResult refreshTokenResult { get; set; }
}

public class RefreshTokenResult
{
    public string UserName { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpireAt { get; set; }
}