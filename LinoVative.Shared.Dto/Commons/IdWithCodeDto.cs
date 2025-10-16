using LinoVative.Shared.Dto.Interfaces;

namespace LinoVative.Shared.Dto.Commons
{
    public class IdWithCodeDto : IEntityDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
