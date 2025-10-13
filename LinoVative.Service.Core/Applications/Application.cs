using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Applications
{
    public class Application : AuditableEntity
    {
        public string? Name { get; set; }
        public Guid? Secreet { get; set; }
    }
}
