using LinoVative.Shared.Dto.Outlets;

namespace LinoVative.Shared.Dto.MasterData.Shifts
{
    public class ShiftUpdateDto : ShiftDto
    {
        public List<OutletShiftDto> Outlets { get; set; } = new();
    }
}
