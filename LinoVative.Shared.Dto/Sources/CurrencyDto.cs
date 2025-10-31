using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.Sources
{
    public class CurrencyDto : EntityDtoBase
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Symbol { get; set; }
        public Guid? RegionId { get; set; }
        public IdWithNameDto? Region { get; set; }
    }
}
