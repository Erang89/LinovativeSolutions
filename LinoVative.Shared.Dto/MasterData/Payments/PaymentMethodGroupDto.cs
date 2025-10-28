using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
namespace LinoVative.Shared.Dto.MasterData.Payments
{

    [LocalizerKey(nameof(PaymentMethodGroupDto))]
    public class PaymentMethodGroupDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField(EntityTypes.PaymentMethodGroup)]
        public string? Name { get; set; }
        
    }

    public class PaymentMethodGroupViewDto : PaymentMethodGroupDto
    {
        public List<PaymentMethodViewDto> PaymentMethods { get; set; } = new();
    }
}
