using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Auth;

namespace LinoVative.Service.Backend.Services
{
    public class UserPrivilegeService : IUserPrivilegeService
    {
        readonly IAppDbContext _dbContext;

        public UserPrivilegeService(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IQueryable<Guid> GetUserCompanyIds(Guid userId)
        {
            var appUserIds = _dbContext.UserCompanies.Where(x => !x.IsDeleted && x.UserId == userId).Select(x => x.CompanyId);
            return _dbContext.Companies.Where(x => !x.IsDeleted && (x.OwnByUserId == userId || appUserIds.Contains(x.Id))).Select(x => x.Id);
        }

        public IQueryable<Guid> GetUserCompanyIds(AppUser user)
        {
            return GetUserCompanyIds(user.Id);
        }

        public IQueryable<Guid> GetUserCompanyIds(IActor actor) => GetUserCompanyIds(actor.UserId);
    }
}
