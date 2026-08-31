using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.DTOs;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Data.Requests;
using SchoolProject.Data.Results;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementions;

public class AuthorizationService : IAuthorizationService
{
    #region Fields
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDBContext _context;
    #endregion

    #region Constructor
    public AuthorizationService(RoleManager<Role> roleManager,
                                UserManager<User> userManager,
                                ApplicationDBContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }
    #endregion

    #region Methods
    public async Task<string> AddRoleAsync(string roleName)
    {
        var identityRole = new Role
        {
            Name = roleName
        };
        var role = await _roleManager.CreateAsync(identityRole);
        return role.Succeeded ? "Success" : "Failed";
    }

    public async Task<string> DeleteRoleAsync(int roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            return "notFound";
        //check if the role is assigned to any user before deleting it
        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
        if (usersInRole.Any())
            return "Used";
        var res = await _roleManager.DeleteAsync(role);
        if(res.Succeeded)
            return "Success";
        var errors = string.Join(", ", res.Errors.Select(e => e.Description));
        return errors;
    }

    public async Task<string> EditRoleAsync(int roleId, string roleName)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            return "notFound";

        role.Name = roleName;
        var res =  await _roleManager.UpdateAsync(role);
        if(res.Succeeded)
            return "Success";
        var errors = string.Join(", ", res.Errors.Select(e => e.Description));
        return errors;
    }

    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        return await _roleManager.FindByIdAsync(roleId.ToString());
    }

    public async Task<List<Role>> GetRolesListAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles;
    }

    public async Task<bool> IsRoleExist(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task<MangeUserClaimsResult?> ManageUserClaimsAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return null;

        var response = new MangeUserClaimsResult
        {
            UserId = userId,
            userClaims = new List<UserClaims>()
        };

        var userClaims = await _userManager.GetClaimsAsync(user);
        foreach (var claim in ClaimsStore.Claims)
        {
            response.userClaims.Add(new UserClaims
            {
                Type = claim.Type,
                Value = userClaims.Any(c => c.Type == claim.Type)
            });
        }
        return response;
    }

    public async Task<ManageUserRolesResult?> ManageUserRolesAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return null;

        var roles = await _roleManager.Roles
            .Select(role => new Roles
            {
                Id = role.Id,
                Name = role.Name
            })
            .ToListAsync();

        var userRoles = await _userManager.GetRolesAsync(user);

        var userRoleSet = userRoles.ToHashSet();

        foreach (var role in roles)
        {
            role.HasRole = userRoleSet.Contains(role.Name);
        }

        return new ManageUserRolesResult
        {
            UserId = userId,
            Roles = roles
        };
    }

    public async Task<string> UpdateUserClaimsAsync(UpdateUserClaimsRequest request)
    {
        var transact = _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
                return "NotFound";

            var userClaims = await _userManager.GetClaimsAsync(user);
            var RemovingResult = await _userManager.RemoveClaimsAsync(user, userClaims);
            if (!RemovingResult.Succeeded)
                return "FailedToRemoveClaims";
            var claimsToAdd = request.userClaims.Where(c => c.Value).Select(c => new Claim(c.Type, c.Value.ToString())).ToList();
            var result = await _userManager.AddClaimsAsync(user, claimsToAdd);
            if (!result.Succeeded)
                return "FailedToAddClaims";
            await transact.Result.CommitAsync();
            return "Success";
        }
        catch (Exception ex)
        {
            await transact.Result.RollbackAsync();
            return $"Error: {ex.Message}";
        }
    }

    public async Task<string> UpdateUserRolesAsync(ManageUserRolesResult request)
    {
        var transact = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
                return "NotFound";

            var userRoles = await _userManager.GetRolesAsync(user);
            var RemovingResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!RemovingResult.Succeeded)
                return "FailedToRemoveRoles";
            var result = await _userManager.AddToRolesAsync(user, request.Roles.Where(r => r.HasRole).Select(r => r.Name));
            if (!result.Succeeded)
                return "FailedToAddRoles";
            transact.Commit();
            return "Success";
        }
        catch (Exception ex)
        {
            await transact.RollbackAsync();
            return $"Error: {ex.Message}";
        }
    }
    #endregion

}
