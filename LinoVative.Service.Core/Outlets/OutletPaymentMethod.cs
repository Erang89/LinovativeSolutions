using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;

namespace LinoVative.Service.Core.Outlets
{
    public class OutletPaymentMethod : AuditableEntity, IUtilizedByOutlet
    {
        public Guid? OutletId { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
        public Outlet? Outlet { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
