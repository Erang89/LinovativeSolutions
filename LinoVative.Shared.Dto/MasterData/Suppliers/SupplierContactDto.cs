using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Suppliers
{
    public class SupplierContactDto : EntityDtoBase
    {
        public Guid SupplierId { get; set; }
        [LocalizedRequired]
        public string ContactName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }
        public bool IsPrimary { get; set; }
    }


    public class SupplierContactViewDto
    {
        public SupplierDto? Supplier { get; set; } = null!;
    }
}
