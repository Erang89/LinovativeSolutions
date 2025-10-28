using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
    }


    public class OutletViewDto : OutletDto 
    {
    }
}
