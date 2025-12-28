using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Suppliers
{

    [LocalizerKey(nameof(SupplierDto))]
    public class SupplierAddressDto : EntityDtoBase
    {
        public Guid SupplierId { get; set; }

        [LocalizedRequired]
        public AddressType? AddressType { get; set; }

        [LocalizedRequired]
        public string AddressLine { get; set; } = null!;

        [LocalizedRequired]
        public string City { get; set; } = null!;

        [LocalizedRequired]
        public string Province { get; set; } = null!;

        [LocalizedRequired]
        public string Country { get; set; } = null!;

        public string? PostalCode { get; set; }
    }


    public class SupplierAddressViewDto : SupplierAddressDto
    {
        public SupplierDto? Supplier { get; set; } = null!;
    }
}
