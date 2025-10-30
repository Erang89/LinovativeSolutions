using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.MasterData.Accountings
{

    [LocalizerKey(nameof(SalesCOAMappingDto))]
    public class SalesCOAMappingDto : EntityDtoBase
    {
        
        [LocalizedRequired, EntityID(EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.Account)]
        public Guid? AccountId { get; set; }

        public bool AutoPushWhenPOSClosing { get; set; }
    }


    public class SalesCOAMappingViewDto : SalesCOAMappingDto
    {
        public IdWithNameDto? Outlet { get; set; }
        public IdWithCodeDto? Account { get; set; }
    }
}
