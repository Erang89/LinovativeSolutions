using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Sources
{
    public class AppTimeZone : AuditableEntity
    {
        public string? TimeZone { get; set; }
        public string? Zona { get; set; }
        public string? GMT { get; set; }
        public string? Name { get; set; }
    }
}
