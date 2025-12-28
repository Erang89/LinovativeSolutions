using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Suppliers
{

    [LocalizerKey(nameof(SupplierDto))]
    public class SupplierContactDto : EntityDtoBase
    {
        public Guid SupplierId { get; set; }

        [LocalizedRequired]
        public string ContactName { get; set; } = null!;

        [LocalizedEmail]
        public string? Email { get; set; }

        [LocalizedPhone]
        public string? Phone { get; set; }

        [LocalizedPhone]
        public string? Whatsapp { get; set; }

        public string? Position { get; set; }
        public bool IsPrimary { get; set; }
    }


    public class SupplierContactViewDto
    {
        public SupplierDto? Supplier { get; set; } = null!;
    }
}
