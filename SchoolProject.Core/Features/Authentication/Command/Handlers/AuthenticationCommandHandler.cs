using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authentication.Command.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authentication.Command.Handlers;

public class AuthenticationCommandHandler : ResponseHandler,
                                          IRequestHandler<SignInCommand, Response<JwtAuthRsult>>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAuthenticationService _authenticationService;
    #endregion

    #region Constructor
    public AuthenticationCommandHandler(IStringLocalizer<SharedResources> localizer,
                                        UserManager<User> userManager,
                                        SignInManager<User> signInManager,
                                        IAuthenticationService authenticationService) : base(localizer)
    {
        _localizer = localizer;
        _userManager = userManager;
        _signInManager = signInManager;
        _authenticationService = authenticationService;
    }
    #endregion

    #region Methods
    public async Task<Response<JwtAuthRsult>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        //check if the user exist
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
            return BadRequest<JwtAuthRsult>(_localizer[SharedResourcesKeys.UserNameIsNotExist]);

        //check if the password is correct
        var isPasswordValid = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!isPasswordValid.Succeeded)            
            return BadRequest<JwtAuthRsult>(_localizer[SharedResourcesKeys.PasswordNotCorrect]);

        //generate token
        var token = await _authenticationService.GetJWTToken(user);

        return Success(token);

    }
    #endregion  
}
