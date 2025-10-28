using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.Outlets
{


    [LocalizerKey(nameof(OutletTableDto))]
    public class OutletTableDto : EntityDtoBase
    {
        public string? Name { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.OutletArea)]
        public Guid? AreaId { get; set; }


        public bool IsActive { get; set; } = true;
    }

    public class OutletTableViewDto : OutletTableDto
    {

        public OutletAreaViewDto? Area { get; set; }
    }
}
