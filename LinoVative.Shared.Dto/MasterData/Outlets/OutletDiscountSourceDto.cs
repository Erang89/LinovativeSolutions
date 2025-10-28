using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.Outlets
{

    [LocalizerKey(nameof(OutletDiscountSourceDto))]
    public class OutletDiscountSourceDto : EntityDtoBase
    {
        [LocalizedRequired, EntityID( EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }

        public decimal DiscountPercent { get; set; }
        public bool ApplyInBillLevel { get; set; }
        public bool ApplyInItemLevel { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class OutletDiscountSourceViewDto : OutletDiscountSourceDto
    {
        public OutletViewDto? Outlet { get; set; }
    }
}
