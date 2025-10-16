using LinoVative.Shared.Dto.Interfaces;

namespace LinoVative.Shared.Dto.Commons
{
    public class IdWithNameDto : IEntityDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
