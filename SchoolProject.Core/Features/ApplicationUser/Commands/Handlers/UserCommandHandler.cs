using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Handlers;

public class UserCommandHandler : ResponseHandler,
                                  IRequestHandler<AddUserCommand, Response<string>>,
                                  IRequestHandler<UpdateUserCommand, Response<string>>,
                                  IRequestHandler<DeleteUserCommand, Response<string>>,
                                  IRequestHandler<ChangeUserPasswordCommand, Response<string>>
{
    #region Fields
    private readonly IMapper mapper;
    private readonly IStringLocalizer<SharedResources> stringLocalizer;
    private readonly UserManager<User> userManager;
    #endregion

    #region constructor
    public UserCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                              IMapper mapper,
                              UserManager<User> userManager):base(stringLocalizer)
    {
        this.mapper = mapper;
        this.stringLocalizer = stringLocalizer;
        this.userManager = userManager; 
    }
    #endregion

    #region Methods
    public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        //if the email already exists, return error message
        var userByEmail = await userManager.FindByEmailAsync(request.Email);
        if (userByEmail != null)
            return BadRequest<string>(stringLocalizer[SharedResourcesKeys.EmailIsExist]);

        //if the username already exists, return error message
        var userByUserName = await userManager.FindByNameAsync(request.UserName);
        if (userByUserName != null)
            return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UserNameIsExist]);    

        //Mapping
        var user = mapper.Map<User>(request);

        var createdUser = await userManager.CreateAsync(user, request.Password);
        if (!createdUser.Succeeded)            
            return BadRequest<string>(createdUser.Errors.FirstOrDefault()?.Description);

        return Created("");
    }

    public async Task<Response<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
        //check if the user exist or not
        if (user == null)
            return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);

         //if the username already exists, return error message
        var userByUserName = await userManager.FindByNameAsync(request.UserName);
        if (userByUserName != null && userByUserName.Id != request.Id)
            return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UserNameIsExist]);  
              
        //mapping
        var updatedUser = mapper.Map(request, user);

        //updating 
        var res = await userManager.UpdateAsync(updatedUser);

        //result is not success
        if(!res.Succeeded)return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UpdateFailed]);
        
        return Success((string)stringLocalizer[SharedResourcesKeys.Updated]);
    }

    public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
            return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);
        var res = await userManager.DeleteAsync(user);
        if (!res.Succeeded) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.DeletedFailed]);
        return Success((string)stringLocalizer[SharedResourcesKeys.Deleted]);
    }

    public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
            return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);

        var isMatch = await userManager.CheckPasswordAsync(user, request.CurrnetPassword);
        if (!isMatch)
            return BadRequest<string>(stringLocalizer[SharedResourcesKeys.PasswordNotCorrect]);

        if (request.NewPassword != request.ConfirmPassword)
            return BadRequest<string>(stringLocalizer[SharedResourcesKeys.PasswordNotEqualConfirmPass]);

        var result = await userManager.ChangePasswordAsync(user, request.CurrnetPassword, request.NewPassword);
        if (!result.Succeeded)
            return BadRequest<string>(result.Errors.FirstOrDefault()?.Description);

        return Success((string)stringLocalizer[SharedResourcesKeys.Success]);
    }
    #endregion
}