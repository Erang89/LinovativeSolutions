using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.OrderTypes
{

    [LocalizerKey(nameof(OrderTypeDto))]
    public class OrderTypeDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField( EntityTypes.OrderType)]
        public string? Name { get; set; }
        public OrderBehaviors Behavior { get; set; }
    }

    public class OrderTypeViewDto : OrderTypeDto
    {

    }
}
