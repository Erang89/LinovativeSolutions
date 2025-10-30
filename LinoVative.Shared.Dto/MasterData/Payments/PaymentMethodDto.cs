using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;

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

}
