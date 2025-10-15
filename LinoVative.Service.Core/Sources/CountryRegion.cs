using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Sources
{
    public class CountryRegion : AuditableEntity
    {
        public string? Name { get; set; }
        public List<Country> Countries { get; set; } = new();
    }
}
