using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.Sources
{
    public class CountryDto : EntityDtoBase
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public Guid? RegionId { get; set; }
        public IdWithNameDto? Region { get; set; }
    }
}
