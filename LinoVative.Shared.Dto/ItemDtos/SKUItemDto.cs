using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.ItemDtos
{
    public class SKUItemDto : EntityDtoBase
    {

        [EntityID(EntityTypes.Item)]

        public Guid ItemId { get; set; }

        public string SKU { get; set; } = null!;

        [EntityID(EntityTypes.ItemUnit)]
        public Guid UnitId { get; set; }

        [EntityID(EntityTypes.ItemCategory)]
        public Guid CategoryId { get; set; }

        [LocalizeMinDecimalValue(0)]
        public decimal SalePrice { get; set; }

        // Sale Tax and service
        public bool HasSaleTaxAndService { get; set; }
        public bool SalePriceIncludeTaxAndService { get; set; }
        public bool UseOutletSaleTaxAndService { get; set; }

        // Purchase Settings
        [LocalizeMinDecimalValue(0)]
        public decimal DefaultPurchaseQty { get; set; }
        [LocalizeMinDecimalValue(0)]
        public decimal MinimumStockQty { get; set; }

        // Costume Price
        public bool HasCostumePrice { get; set; }

        public IdWithNameDto? Unit { get; set; } 
        public ItemCategoryViewDto? Category { get; set; }

    }

    public class SKUItemViewDto : SKUItemDto
    {
        public string ItemCode { get; set; } = null!;
        public string ItemName { get; set; } = null!;
    }

    public class SKUItemInputDto : SKUItemDto
    {
        public List<ItemPriceTypeDto> CostumePrices { get; set; } = new();
    }
}
