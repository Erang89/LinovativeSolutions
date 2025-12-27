using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Suppliers
{
    public class SupplierAddress : AuditableEntity
    {
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public AddressType AddressType { get; set; }

        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? PostalCode { get; set; }
    }
}
