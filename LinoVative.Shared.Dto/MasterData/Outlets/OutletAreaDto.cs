using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.Outlets
{

    [LocalizerKey(nameof(OutletAreaDto))]
    public class OutletAreaDto : EntityDtoBase
    {
        [LocalizedRequired, EntityID(EntityTypes.OutletArea)]
        public Guid? AreaId { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.Outlet)]
        public Guid? OutletId { get; set; }

        public string? Name { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
    }

   

    public class OutletAreaViewDto : OutletAreaDto
    {
        public OutletViewDto? Outlet { get; set; }
        public List<OutletTableViewDto> Tables { get; set; } = new();
    }
}
