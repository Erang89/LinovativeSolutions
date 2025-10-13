using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Sources;

namespace LinoVative.Service.Core.Companies
{
    public class Company : AuditableEntity
    {
        public string? Name { get; set; }
        public string? Address { get; set; }

        public Guid? CountryId { get; set; }
        public Country? Country { get; set; }
        public string? TimeZone { get; set; }
        public Guid? OwnByUserId { get; set; }
        public Guid? CurrencyId { get; set; }
        public Currency? Currency { get; set; }
    }
}
