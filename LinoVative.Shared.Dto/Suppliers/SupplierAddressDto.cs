using Linovative.Shared.Interface.Enums;

namespace LinoVative.Shared.Dto.Suppliers
{
    public class SupplierAddressDto : EntityDtoBase
    {
        public Guid SupplierId { get; set; }

        public AddressType AddressType { get; set; }

        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? PostalCode { get; set; }
    }


    public class SupplierAddressViewDto : SupplierAddressDto
    {
        public SupplierDto? Supplier { get; set; } = null!;
    }
}
