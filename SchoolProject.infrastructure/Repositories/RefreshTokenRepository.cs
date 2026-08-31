using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.InfrastructureBases;

namespace SchoolProject.Infrastructure.Repositories;

public class RefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IRefreshTokenRepository
{
     #region Fields
        private readonly DbSet<UserRefreshToken> _refreshTokens;
        #endregion

        #region Constructor
        public RefreshTokenRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _refreshTokens = dbContext.Set<UserRefreshToken>();
        }
        #endregion

        #region Implemention    
        public async Task<List<UserRefreshToken>> GetRefreshTokensListAsync()
        {
            return await _refreshTokens.ToListAsync();
        }
        #endregion
}
