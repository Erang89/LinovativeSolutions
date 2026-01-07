using LinoVative.Shared.Dto.Interfaces;

namespace LinoVative.Shared.Dto.Commons
{
    public class IdWithNameDto : EntityDtoBase, IEntityDto
    {
        public virtual string? Name { get; set; }
    }
}
