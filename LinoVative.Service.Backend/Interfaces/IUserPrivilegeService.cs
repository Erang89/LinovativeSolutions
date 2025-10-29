using Linovative.Shared.Interface;
using LinoVative.Service.Core.Auth;

namespace LinoVative.Service.Backend.Interfaces
{
    public interface IUserPrivilegeService
    {
        IQueryable<Guid> GetUserCompanyIds(Guid userId);
        IQueryable<Guid> GetUserCompanyIds(AppUser user);
        IQueryable<Guid> GetUserCompanyIds(IActor actor);
    }
}
