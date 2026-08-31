using System.Security.Claims;

namespace SchoolProject.Data.Helpers;

public static class ClaimsStore
{
    public static List<Claim> Claims = new()
    {
        new Claim("Create Student", "true"),
        new Claim("Edit Student", "false"),
        new Claim("Delete Student", "false")
    };
}
