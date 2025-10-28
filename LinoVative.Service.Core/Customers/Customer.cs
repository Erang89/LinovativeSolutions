using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.People;

namespace LinoVative.Service.Core.Customers
{
    public class Customer : AuditableEntityUnderCompany
    {
        public Guid? PersonId { get; set; }
        public string? CustomerCode { get; set; }

        public Person? Person { get; set; }
    }
}
