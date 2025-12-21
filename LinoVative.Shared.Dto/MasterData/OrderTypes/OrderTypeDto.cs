using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Outlets;

namespace LinoVative.Shared.Dto.OrderTypes
{

    [LocalizerKey(nameof(OrderTypeDto))]
    public class OrderTypeDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField( EntityTypes.OrderType)]
        public string? Name { get; set; }

        [LocalizedRequired]
        public OrderBehaviors? Behavior { get; set; }
    }

    public class OrderTypeInputDto : OrderTypeDto
    {
        public List<OutletOrderTypeDto> OutletOrderTypes { get; set; } = new();
    }

    public class OrderTypeViewDto : OrderTypeDto
    {

    }
}
