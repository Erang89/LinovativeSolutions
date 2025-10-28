using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.MasterData.Users;

namespace LinoVative.Shared.Dto.Outlets
{

    [LocalizerKey(nameof(OutletUserDto))]
    public class OutletUserDto : EntityDtoBase
    {
        [LocalizedRequired, EntityID(EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }


        [LocalizedRequired, EntityID(EntityTypes.AppUser)]
        public Guid? UserId { get; set; }


        public bool IsActive { get; set; } = true;
    }


    public class OutletUserViewDto : OutletUserDto
    {
        public OutletViewDto? Outlet { get; set; }
        public UserViewDto? User { get; set; }
    }
}
