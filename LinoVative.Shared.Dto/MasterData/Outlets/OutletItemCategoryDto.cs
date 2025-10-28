using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.ItemDtos;

namespace LinoVative.Shared.Dto.Outlets
{


    [LocalizerKey(nameof(OutletItemCategoryDto))]
    public class OutletItemCategoryDto : EntityDtoBase
    {

        [LocalizedRequired, EntityID(EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.ItemCategory)]
        public Guid? ItemCategoryId { get; set; }

        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
    }


    public class OutletItemCategoryViewDto : OutletItemCategoryDto
    {

        public OutletViewDto? Outlet { get; set; }
        public ItemCategoryViewDto? ItemCategory { get; set; }
    }
}
