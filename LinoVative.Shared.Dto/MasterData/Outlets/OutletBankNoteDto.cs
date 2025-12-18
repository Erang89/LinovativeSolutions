using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.MasterData.Payments;

namespace LinoVative.Shared.Dto.Outlets
{

    [LocalizerKey(nameof(OutletBankNoteDto))]
    public class OutletBankNoteDto : EntityDtoBase, IActiveFlag, IHasSequence
    {

        [LocalizedRequired, EntityID(EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.BankNote)]
        public Guid? BankNoteId { get; set; }

        public bool IsActive { get; set; }
        public int Sequence { get; set; }
        public BankNoteDto? BankNote { get; set; }
    }
}
