using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;

namespace LinoVative.Service.Core.Outlets
{
    public class OutletItemGroup : AuditableEntity, IUtilizedByOutlet
    {
        public Guid? OutletId { get; set; }
        public Guid? ItemGroupId { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
        public Outlet? Outlet { get; set; }
        public ItemGroup? ItemGroup { get; set; }
    }
}
