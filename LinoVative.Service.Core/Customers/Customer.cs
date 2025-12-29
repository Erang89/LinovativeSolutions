using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.People;

namespace LinoVative.Service.Core.Customers
{
    public class Customer : AuditableEntityUnderCompany
    {
        public Guid? PersonId { get; set; }
        public string? CustomerCode { get; set; }

        public Person? Person { get; set; }

        public string? LegalName { get; set; }

        public CustomerType CustomerType { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public bool IsMember { get; set; }
        public string? MemberNumber { get; set; }
        public int Points { get; set; }

        public string? TaxNumber { get; set; }
        public decimal? CreditLimit { get; set; }
        public bool AllowCredit { get; set; }
        public string? PaymentTerm { get; set; }
    }
}
