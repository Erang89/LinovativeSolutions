using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Payments
{

    [LocalizerKey(nameof(BankNoteDto))]
    public class BankNoteDto : EntityDtoBase
    {
        [LocalizedRequired]
        public decimal Note { get; set; }
    }
}
