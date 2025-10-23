using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.ItemDtos
{

    public class ItemGroupDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField(EntityTypes.ItemGroup)]
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
