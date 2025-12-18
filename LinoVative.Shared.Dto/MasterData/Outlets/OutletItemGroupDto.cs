using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.ItemDtos;

namespace LinoVative.Shared.Dto.Outlets
{


    [LocalizerKey(nameof(OutletItemGroupDto))]
    public class OutletItemGroupDto : EntityDtoBase, IHasSequence
    {

        [LocalizedRequired, EntityID(EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.ItemGroup)]
        public Guid? ItemGroupId { get; set; }

        public bool UseAllItemCategories { get; set; } = true;

        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;       
    }

    public class OutletItemGroupViewDto : OutletItemGroupDto
    {
        public OutletViewDto? Outlet { get; set; }
        public ItemGroupViewDto? ItemGroup { get; set; }
    }
}
