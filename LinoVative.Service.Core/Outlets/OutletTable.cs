using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Core.Outlets
{
    public class OutletTable : AuditableEntity
    {
        public string? Name { get; set; }
        public Guid? AreaId { get; set; }
        public bool IsActive { get; set; } = true;
        public OutletArea? Area { get; set; }
    }
}
