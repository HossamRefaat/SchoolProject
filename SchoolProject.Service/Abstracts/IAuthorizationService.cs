using SchoolProject.Data.DTOs;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Requests;
using SchoolProject.Data.Results;

namespace SchoolProject.Service.Abstracts;

public interface IAuthorizationService
{
    public Task<string> AddRoleAsync(string roleName);
    public Task<bool> IsRoleExist(string roleName);
    public Task<string> EditRoleAsync(int roleId, string roleName);
    public Task<List<Role>> GetRolesListAsync();
    public Task<Role?> GetRoleByIdAsync(int roleId);
    public Task<string> DeleteRoleAsync(int roleId);
    public Task<ManageUserRolesResult?> ManageUserRolesAsync(int userId);
    public Task<string> UpdateUserRolesAsync(ManageUserRolesResult request);
    public Task<MangeUserClaimsResult?> ManageUserClaimsAsync(int userId);
    public Task<string> UpdateUserClaimsAsync(UpdateUserClaimsRequest request);

}

