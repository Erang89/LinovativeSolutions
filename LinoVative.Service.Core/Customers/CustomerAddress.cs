using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Sources;

namespace LinoVative.Service.Core.Customers
{
    public class CustomerAddress : AuditableEntity
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public CustomerAddressType AddressType { get; set; }

        public Guid CountryId { get; set; }
        public Country Country { get; set; } = null!;

        public string AddressLine { get; set; } = null!;
        public string? City { get; set; } = null!;
        public string? ProvinceName { get; set; } = null!;
        public string? PostalCode { get; set; }

        public Guid? ProvinceId { get; set; }
        public Guid? RegencyId { get; set; }
        public Regency? Regency { get; set; }
    }
}
