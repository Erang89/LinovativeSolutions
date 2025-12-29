namespace LinoVative.Shared.Dto.Sources
{
    public class RegencyDto : EntityDtoBase
    {
        public Guid ProvinceId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
