using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.OrderTypes;

namespace LinoVative.Service.Core.Outlets
{
    public class OutletOrderType : AuditableEntity, IUtilizedByOutlet
    {
        public Guid? OutletId { get; set; }
        public Guid? OrderTypeId { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
        public bool UseDefaultTaxAndService { get; set; } = true;
        public decimal TaxPercent { get; set; }
        public decimal ServicePercent { get; set; }
        public Outlet? Outlet { get; set; }
        public OrderType? OrderType { get; set; }
    }
}
