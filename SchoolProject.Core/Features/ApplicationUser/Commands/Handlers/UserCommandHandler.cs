using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Handlers;

public class UserCommandHandler : ResponseHandler, IRequestHandler<AddUserCommand, Response<string>>
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
    #endregion
}
