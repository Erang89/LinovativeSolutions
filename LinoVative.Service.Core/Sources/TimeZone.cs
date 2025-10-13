using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Sources
{
    public class AppTimeZone : AuditableEntity
    {
        public string? TimeZone { get; set; }
    }
}
