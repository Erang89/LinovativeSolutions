using LinoVative.Shared.Dto.Attributes;
using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.ItemDtos
{

    [LocalizerKey(nameof(ItemDto))]
    public class ItemDto : EntityDtoBase
    {

        [LocalizedRequired, UniqueField(EntityTypes.Item)]
        public string? Code { get; set; }
        
        [LocalizedRequired, UniqueField(EntityTypes.Item)]
        public string? Name { get; set; }

        public string? Description { get; set; }




        [LocalizedRequired, EntityID(EntityTypes.ItemUnit)]
        public Guid? UnitId { get; set; }
        
        [LocalizedRequired, EntityID(EntityTypes.ItemCategory)]
        public Guid? CategoryId { get; set; }
       


        public bool IsActive { get; set; } = true;
        

        // Tax and Service
        public bool HasSellingTaxAndService { get; set; }
        public bool SellPriceIncludeTaxService { get; set; }
        public bool TaxAndServicePercentFromOutletOrderType { get; set; }
        [LocalizeMinDecimalValue(0), LocalizeMaxDecimalValue(100)]
        public decimal? ServicePercent { get; set; }
        [LocalizeMinDecimalValue(0), LocalizeMaxDecimalValue(100)]
        public decimal? TaxPercent { get; set; }


        // Prices

        [LocalizedRequired, LocalizeMinDecimalValue(0)]
        public decimal? SellPrice { get; set; }
        public bool HasCostumePrice { get; set; }


        // Purchase
        public bool CanBePurchased { get; set; }
        [LocalizeMinDecimalValue(0)]
        public decimal? DefaltPurchaseQty { get; set; }
        [LocalizeMinDecimalValue(0)]
        public decimal? ShouldPurchaseWhenStockLessOrEqualsTo { get; set; }

    }





    public class ItemViewDto : ItemDto
    {
        public IdWithNameDto? Unit { get; set; }
        public ItemCategoryViewDto? Category { get; set; }

    }




    public class ItemInputDto : ItemViewDto
    {
        public List<ItemPriceTypeDto> ItemPriceTypes { get; set; } = new();
    }
}
