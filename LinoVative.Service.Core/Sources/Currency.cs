using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Sources
{
    public class Currency : AuditableEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Symbol { get; set; }
    }
}
