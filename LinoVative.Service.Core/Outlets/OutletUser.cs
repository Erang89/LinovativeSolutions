using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Core.Outlets
{
    public class OutletUser : AuditableEntity, IUtilizedByOutlet
    {
        public Guid? OutletId { get; set; }
        public Guid? UserId { get; set; }
        public bool IsActive { get; set; } = true;
        public Outlet? Outlet { get; set; }
        public AppUser? User { get; set; }
    }
}
