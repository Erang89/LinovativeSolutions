using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Payments
{
    public class BankNote : AuditableEntityUnderCompany
    {
        public decimal Note { get; set; }
    }
}
