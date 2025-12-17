using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.MasterData.Shifts;

namespace LinoVative.Shared.Dto.Outlets
{
    [LocalizerKey(nameof(OutletShiftDto))]
    public class OutletShiftDto : EntityDtoBase
    {

        [LocalizedRequired, EntityID(EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.Shift)]
        public Guid? ShiftId { get; set; }

        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;

        [LocalizedRequired]
        public TimeSpan? StartTime { get; set; }

        [LocalizedRequired]
        public TimeSpan? EndTime { get; set; }
    }


    public class OutletShiftViewDto : OutletShiftDto
    {
        public IdWithNameDto? Shift { get; set; }
        public IdWithNameDto? Outlet { get; set; }
    }
}
