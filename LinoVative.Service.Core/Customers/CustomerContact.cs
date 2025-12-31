using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Customers
{
    public class CustomerContact : AuditableEntity
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Whatsapp { get; set; }
        public string? Position { get; set; }
        public bool IsPrimary { get; set; }
    }
}