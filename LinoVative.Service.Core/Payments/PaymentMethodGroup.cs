using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Payments
{
    public class PaymentMethodGroup : AuditableEntityUnderCompany
    {
        public string? Name { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; } = new();
    }
}
