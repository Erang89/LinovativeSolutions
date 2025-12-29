using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.Sources
{
    public class RegencyDto : EntityDtoBase
    {
        [EntityID(EntityTypes.Province)]
        public Guid ProvinceId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
