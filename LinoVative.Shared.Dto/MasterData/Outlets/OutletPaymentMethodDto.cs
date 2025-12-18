using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.Outlets
{


    [LocalizerKey(nameof(OutletPaymentMethodDto))]
    public class OutletPaymentMethodDto : EntityDtoBase, IHasSequence, IActiveFlag
    {

        [LocalizedRequired, EntityID(EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.PaymentMethod)]
        public Guid? PaymentMethodId { get; set; }


        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;        
    }

    public class OutletPaymentMethodViewDto : OutletPaymentMethodDto
    {
        public IdWithNameDto? Outlet { get; set; }
        public IdWithNameDto? PaymentMethod { get; set; }
    }
}
