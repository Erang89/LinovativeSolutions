using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Payments
{
    public class PaymentMethod : AuditableEntityUnderCompany
    {
        public Guid? PaymentMethodGroupId { get; set; }
        public string Name { get; set; } = default!;
        public PaymentMethodTypes Type { get; set; }
        public PaymentMethodGroup? PaymentMethodGroup { get; set; }
    }
}
