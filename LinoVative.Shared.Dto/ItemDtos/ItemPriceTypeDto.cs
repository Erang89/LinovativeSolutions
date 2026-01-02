using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.ItemDtos
{

    [LocalizerKey(nameof(ItemPriceTypeDto))]
    public class ItemPriceTypeDto : EntityDtoBase
    {

        [EntityID(EntityTypes.Item)]
        public Guid? SKUItemId { get; set; }

        [LocalizeMinDecimalValue(0)]
        public decimal? Price { get; set; }

        [EntityID(EntityTypes.PriceType)]
        public Guid? PriceTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
