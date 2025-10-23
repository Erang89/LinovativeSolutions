using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.ItemDtos
{
    public class ItemCategoryDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField(EntityTypes.ItemCategory)]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [LocalizedRequired]
        public Guid? ItemGroupId { get; set; }
    }

    public class ItemCategoryViewDto : ItemCategoryDto {
        public IdWithNameDto? ItemGroup { get; set; }
    }
}
