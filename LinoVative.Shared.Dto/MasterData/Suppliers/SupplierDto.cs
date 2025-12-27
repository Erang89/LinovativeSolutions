using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Suppliers
{

    [LocalizerKey(nameof(SupplierDto))]
    public class SupplierDto : EntityDtoBase
    {
        [LocalizedRequired]
        public string Code { get; set; } = null!;

        [LocalizedRequired]
        public string Name { get; set; } = null!;
        public string? LegalName { get; set; }

        [LocalizedRequired]
        public SupplierType? SupplierType { get; set; }

        public string? TaxNumber { get; set; }
        public string? PaymentTerm { get; set; }
        public string? Currency { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Notes { get; set; }
    }


    public class SupplierInputDto : SupplierDto
    {
        public List<SupplierContactDto> Contacts { get; set; } = new List<SupplierContactDto>();
        public List<SupplierAddressDto> Addresses { get; set; } = new List<SupplierAddressDto>();
    }

}
