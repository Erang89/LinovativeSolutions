using LinoVative.Shared.Dto.Attributes;
using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.ItemDtos
{

    [LocalizerKey(nameof(ItemDto))]
    public class ItemDto : EntityDtoBase
    {

        [LocalizedRequired, UniqueField(EntityTypes.Item)]
        public string? Name { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        [LocalizedRequired, EntityID(EntityTypes.ItemGroup)]
        public Guid? CategoryId { get; set; }

        // Selling Settings
        public bool CanBeSell { get; set; }
        [LocalizeMinDecimalValue(0), LocalizeMaxDecimalValue(100)]
        public decimal? DefaultSellServicePercent { get; set; }
        [LocalizeMinDecimalValue(0), LocalizeMaxDecimalValue(100)]
        public decimal? DefaultSellTaxPercent { get; set; }

        // Purchasing Settings
        public bool CanBePurchased { get; set; }

        [LocalizeMinDecimalValue(0), LocalizeMaxDecimalValue(100)]
        public decimal? DefaltPurchaseQty { get; set; }
        [LocalizeMinDecimalValue(0), LocalizeMaxDecimalValue(100)]
        public decimal? DefaultMinimumStock { get; set; }


        // SKU Settings
        public bool HasVariant { get; set; }
    }





    public class ItemViewDto : ItemDto
    {
        
    }


    public class ItemInputDto : ItemViewDto
    {
        public List<SKUItemInputDto> SKUItems { get; set; } = new();
        public IdWithNameDto Category { get; set; } = null!;
    }
}
