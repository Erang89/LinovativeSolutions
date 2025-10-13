using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Auth
{
    public class AppUserApplication : AuditableEntity
    {
        public Guid? UserId { get; set; }
        public Guid? ApplicationId { get; set; }
    }
}
