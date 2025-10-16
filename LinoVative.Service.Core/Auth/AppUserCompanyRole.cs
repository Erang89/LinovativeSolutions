using Linovative.Shared.Cores.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Auth
{
    public class AppUserCompanyRole : AuditableEntity
    {
        public Guid? UserId { get; set; }
        public CompanyRole Role { get; set; }
        public Guid CompanyId { get; set; }
    }
}
