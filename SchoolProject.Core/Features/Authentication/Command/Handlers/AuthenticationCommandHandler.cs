using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authentication.Command.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authentication.Command.Handlers;

public class AuthenticationCommandHandler : ResponseHandler,
                                          IRequestHandler<SignInCommand, Response<JwtAuthRsult>>,
                                          IRequestHandler<RefreshTokenCommand, Response<JwtAuthRsult>>
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

    public async Task<Response<JwtAuthRsult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var jwtToken = _authenticationService.ReadJWTToken(request.AccessToken);    
        var userIdAndExpiryDate = await _authenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);
        switch(userIdAndExpiryDate)
        {
            case ("Algorithm is not valid", null):
                return Unauthorized<JwtAuthRsult>(_localizer[SharedResourcesKeys.AlgorithmIsWrong]);
            case ("Access token is not expired yet", null):
                return BadRequest<JwtAuthRsult>(_localizer[SharedResourcesKeys.TokenIsNotExpired]);
            case ("Refresh token does not exist", null):
                return BadRequest<JwtAuthRsult>(_localizer[SharedResourcesKeys.RefreshTokenIsNotFound]);
            case ("Refresh token is expired", null):
                return BadRequest<JwtAuthRsult>(_localizer[SharedResourcesKeys.RefreshTokenIsExpired]);
        }
        var (userId, expiryDate) = userIdAndExpiryDate;
        var user = await _userManager.FindByIdAsync(userId);
        if(user == null)
            return NotFound<JwtAuthRsult>(_localizer[SharedResourcesKeys.UserIsNotFound]);
        
        var result = await _authenticationService.GetRefreshToken(user, jwtToken, expiryDate, request.RefreshToken);
        return Success(result);
    }
    #endregion
}
