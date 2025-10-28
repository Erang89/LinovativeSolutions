using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Core.Outlets
{
    public class OutletArea : AuditableEntity, IUtilizedByOutlet
    {
        public string? Name { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid? OutletId { get; set; }

        public Outlet? Outlet { get; set; }
        public List<OutletTable> Tables { get; set; } = new();
    }
}
