using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.Outlets;

namespace LinoVative.Shared.Dto.ItemDtos
{

    [LocalizerKey(nameof(ItemCategoryDto))]
    public class ItemCategoryDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField(EntityTypes.ItemCategory)]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.ItemGroup)]
        public Guid? GroupId { get; set; }
    }

    public class ItemCategoryViewDto : ItemCategoryDto {
        public IdWithNameDto? ItemGroup { get; set; }
    }

    public class ItemCategoryInputDto : ItemCategoryDto
    {
        public List<OutletItemCategoryDto> OutletCategories { get; set; } = new();
    }
}
