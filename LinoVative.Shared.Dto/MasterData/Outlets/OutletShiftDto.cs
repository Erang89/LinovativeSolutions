using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.MasterData.Outlets;

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
        public ShiftViewDto? Shift { get; set; }
        public OutletViewDto? Outlet { get; set; }
    }
}
