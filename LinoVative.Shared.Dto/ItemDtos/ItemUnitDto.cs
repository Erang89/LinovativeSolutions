using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.ItemDtos
{

    [LocalizerKey(nameof(ItemUnitDto))]
    public class ItemUnitDto : IdWithNameDto
    {
        [LocalizedRequired, UniqueField(EntityTypes.ItemUnit)]
        public override string? Name { get; set; }
        public string? Description { get; set; }
    }
}
