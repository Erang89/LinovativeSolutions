using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Suppliers
{
    public class SupplierContact : AuditableEntity
    {
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public string ContactName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }

        public bool IsPrimary { get; set; }
    }
}
