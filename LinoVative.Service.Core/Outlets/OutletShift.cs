using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Shifts;

namespace LinoVative.Service.Core.Outlets
{
    public class OutletShift : AuditableEntity, IUtilizedByOutlet
    {
        public Guid? OutletId { get; set; }
        public Guid? ShiftId { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public Shift? Shift { get; set; }
        public Outlet? Outlet { get; set; }
    }
}
