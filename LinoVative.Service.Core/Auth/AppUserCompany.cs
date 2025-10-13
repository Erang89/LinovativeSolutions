using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Auth
{
    public class AppUserCompany : AuditableEntity
    {
        public Guid? CompanyId { get; set; }
        public Guid? UserId { get; set; }
        public bool ApprovedByUser { get; set; }
    }
}
