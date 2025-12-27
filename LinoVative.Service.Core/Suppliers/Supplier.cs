using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Suppliers
{
    public class Supplier : AuditableEntityUnderCompany
    {

        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? LegalName { get; set; }

        public SupplierType SupplierType { get; set; }

        public string? TaxNumber { get; set; }
        public string? PaymentTerm { get; set; }
        public string? Currency { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Notes { get; set; }

        // Navigation
        public ICollection<SupplierContact> Contacts { get; set; } = new List<SupplierContact>();
        public ICollection<SupplierAddress> Addresses { get; set; } = new List<SupplierAddress>();
    }
}
