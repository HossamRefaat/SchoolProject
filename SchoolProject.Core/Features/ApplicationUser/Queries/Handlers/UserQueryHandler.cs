using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Queries.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Queries.Handlers;

public class UserQueryHandler : ResponseHandler,
                                IRequestHandler<GetUserListQuery, PaginatedResult<GetUserListResponse>>,
                                IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> localizer;
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    #endregion

    #region Constructor
    public UserQueryHandler(IStringLocalizer<SharedResources> localizer,
                            IMapper mapper,
                            UserManager<User> userManager) : base(localizer)
    {
        this.localizer = localizer;
        this.mapper = mapper;
        this.userManager = userManager;
    }

   
    #endregion

    #region Methods
    public async Task<PaginatedResult<GetUserListResponse>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var users =  userManager.Users.AsQueryable();
        var paginatedList = await mapper.ProjectTo<GetUserListResponse>(users)
                                        .ToPaginatedListAsync(request.PageNumer, request.PageSize);
        return paginatedList;
    }

    public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            return NotFound<GetUserByIdResponse>(localizer[SharedResourcesKeys.NotFound]);
        var mappedUser = mapper.Map<GetUserByIdResponse>(user);
        return Success(mappedUser);
    }
    #endregion
}
