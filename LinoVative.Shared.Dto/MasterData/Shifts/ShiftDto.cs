using LinoVative.Shared.Dto.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LinoVative.Shared.Dto.MasterData.Outlets
{

    [LocalizerKey(nameof(ShiftDto))]
    public class ShiftDto : EntityDtoBase
    {
        [LocalizedRequired]
        public string? Name { get; set; }

        [LocalizedRequired]
        public TimeSpan? StartTime { get; set; }
        public string? StartTimeFormatted  => StartTime?.ToString(@"hh\:mm\:ss");


        [LocalizedRequired]
        public TimeSpan? EndTime { get; set; }
        public string? EndTimeFormatted => StartTime?.ToString(@"hh\:mm\:ss");


        public bool IsActive { get; set; }
        public int Sequence { get; set; }
    }


    public class ShiftViewDto : ShiftDto { }
}
