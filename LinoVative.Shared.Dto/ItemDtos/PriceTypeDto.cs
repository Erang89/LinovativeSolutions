using LinoVative.Shared.Dto.Attributes;
using Linovative.Shared.Interface.Enums;

namespace LinoVative.Shared.Dto.ItemDtos
{

    [LocalizerKey(nameof(ItemUnitDto))]
    public class PriceTypeDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField(EntityTypes.ItemUnit)]
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
