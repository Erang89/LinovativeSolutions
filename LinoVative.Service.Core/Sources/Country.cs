using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Sources
{
    public class Country : AuditableEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public Guid? RegionId { get; set; }
        public CountryRegion? Region { get; set; }
    }
}
