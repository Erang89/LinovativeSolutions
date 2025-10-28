using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.OrderTypes;

namespace LinoVative.Shared.Dto.Outlets
{

    [LocalizerKey(nameof(OutletTaxAndServiceDto))]
    public class OutletTaxAndServiceDto : EntityDtoBase
    {


        [LocalizedRequired, EntityID(EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }


        [LocalizedRequired, EntityID(EntityTypes.OrderType)]
        public Guid? OrderTypeId { get; set; }


        public decimal TaxPercent { get; set; }
        public decimal ServicePercent { get; set; }
    }

    public class OutletTaxAndServiceViewDto : OutletTaxAndServiceDto
    {
        public OutletViewDto? Outlet { get; set; }
        public OrderTypeViewDto? OrderType { get; set; }
    }
}
