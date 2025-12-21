using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Outlets;

namespace LinoVative.Shared.Dto.MasterData.Payments
{

    [LocalizerKey(nameof(BankNoteDto))]
    public class BankNoteDto : EntityDtoBase
    {
        [LocalizedRequired, LocalizeMinDecimalValue(0.01f)]
        public decimal? Note { get; set; }
    }

    public class BankNoteInputDto : BankNoteDto
    {
        public List<OutletBankNoteDto> OutletBankNotes { get; set; } = new();
    }
}
