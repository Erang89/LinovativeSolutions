using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Outlets;

namespace LinoVative.Shared.Dto.ItemDtos
{
    [LocalizerKey(nameof(ItemGroupDto))]
    public class ItemGroupDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField(EntityTypes.ItemGroup)]
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class ItemGroupViewDto : ItemGroupDto { }


    public class ItemGroupInputDto : ItemGroupDto
    {
        public List<OutletItemGroupDto> OutletGroups { get; set; } = new();
    }
}
