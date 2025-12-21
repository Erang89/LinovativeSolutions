using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.Outlets;

namespace LinoVative.Shared.Dto.MasterData.Payments
{

    [LocalizerKey(nameof(PaymentMethodDto))]
    public class PaymentMethodDto : EntityDtoBase
    {
        [LocalizedRequired]
        public Guid? PaymentMethodGroupId { get; set; }
        [LocalizedRequired, UniqueField(EntityTypes.PaymentMethod)]
        public string? Name { get; set; }
        [LocalizedRequired]
        public PaymentMethodTypes? Type { get; set; }
    }

    public class PaymentMethodViewDto : PaymentMethodDto
    {
        public IdWithNameDto? PaymentMethodGroup { get; set; }
    }

    public class PaymentMethodUpdateDto : PaymentMethodViewDto
    {
        public List<OutletPaymentMethodDto> OutletPaymentMethods { get; set; } = new();
    }

}
