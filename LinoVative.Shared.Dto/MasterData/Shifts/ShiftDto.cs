using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Outlets
{

    [LocalizerKey(nameof(ShiftDto))]
    public class ShiftDto : EntityDtoBase
    {
        [LocalizedRequired]
        public string? Name { get; set; }
        [LocalizedRequired]
        public TimeSpan? StartTime { get; set; }
        [LocalizedRequired]
        public TimeSpan? EndTime { get; set; }
        public bool IsActive { get; set; }
        public int Sequence { get; set; }
    }


    public class ShiftViewDto : ShiftDto { }
}
