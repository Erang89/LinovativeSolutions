using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Shared.Dto.ItemDtos
{
    [LocalizerKey(nameof(SKUItemDto))]
    public class SKUItemDto : EntityDtoBase
    {
        public Guid ItemId { get; set; }

        [LocalizedRequired, UniqueField(EntityTypes.SKUItem)]
        public string SKU { get; set; } = null!;

        public string VarianName { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        [EntityID(EntityTypes.ItemUnit), LocalizedRequired]
        public Guid? UnitId { get; set; }

        [LocalizeMinDecimalValue(0), LocalizedRequired]
        public decimal? SalePrice { get; set; }

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

    }

    public class SKUItemViewDto : SKUItemDto
    {
        public string ItemName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public string UnitName { get; set; } = null!;
        public bool IsPurchaseItem { get; set; }
        public bool IsSaleItem { get; set; }
        public bool ItemDescription { get; set; }
        public ItemDto? Item { get; set; }
    }

    public class SKUItemInputDto : SKUItemDto
    {
        public ItemUnitDto? Unit { get; set; }
        public List<ItemPriceTypeDto> CostumePrices { get; set; } = new();
    }
}
