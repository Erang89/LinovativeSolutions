using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.MasterData.Outlets;

namespace LinoVative.Shared.Dto.Outlets
{
    [LocalizerKey(nameof(OutletDto))]
    public class OutletDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField(EntityTypes.Outlet)]
        public string? Name { get; set; }

        [LocalizedRequired]
        public OutletTypes? OutletType { get; set; }

        public decimal DefaultTaxPercent { get; set; }
        public decimal DefaultServicePercent { get; set; }

        public List<OutletShiftDto> Shifts { get; set; } = new();
        public List<OutletBankNoteDto> BankNotes { get; set; } = new();
        public List<OutletPaymentMethodDto> PaymentMethods { get; set; } = new();
        public List<OutletOrderTypeDto> OrderTypes { get; set; } = new();
        public List<OutletItemExceptionalDto> OutletItemExceptionals { get; set; } = new();
        public List<OutletItemGroupDto> ItemGroups { get; set; } = new();
        public List<OutletItemCategoryDto> ItemCategories { get; set; } = new();

        public bool UseAllItemGroup { get; set; } = true;

    }


    public class OutletViewDto : OutletDto 
    {

    }
}
