namespace SchoolProject.Data.DTOs;

public class ManageUserRolesResult
{
    public int UserId { get; set; }
    public List<Roles> Roles { get; set; } = new List<Roles>();
}

public class Roles
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool HasRole { get; set; }
}