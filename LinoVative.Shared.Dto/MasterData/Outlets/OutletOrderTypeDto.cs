using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.OrderTypes;

namespace LinoVative.Shared.Dto.Outlets
{

    [LocalizerKey(nameof(OutletOrderTypeDto))]
    public class OutletOrderTypeDto : EntityDtoBase, IHasSequence, IActiveFlag
    {
        [LocalizedRequired, EntityID(EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.OrderType)]
        public Guid? OrderTypeId { get; set; }


        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;

        public bool UseDefaultTaxAndService { get; set; } = true;
        public decimal TaxPercent { get; set; }
        public decimal ServicePercent { get; set; }
    }

    public class OutletOrderTypeViewDto : OutletOrderTypeDto
    {

        public IdWithNameDto? Outlet { get; set; }
        public IdWithNameDto? OrderType { get; set; }
    }
}
