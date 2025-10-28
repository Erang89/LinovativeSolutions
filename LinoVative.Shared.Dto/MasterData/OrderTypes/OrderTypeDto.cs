using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.ItemDtos;

namespace LinoVative.Shared.Dto.OrderTypes
{

    [LocalizerKey(nameof(OrderTypeDto))]
    public class OrderTypeDto : EntityDtoBase
    {
        [LocalizedRequired]
        public string? Name { get; set; }
        public OrderBehaviors Behavior { get; set; }
    }

    public class OrderTypeViewDto : OrderTypeDto
    {

    }
}
