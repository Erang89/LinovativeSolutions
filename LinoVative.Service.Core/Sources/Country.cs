using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Sources
{
    public class Country : AuditableEntity
    {
        public string? Name { get; set; }
    }
}
