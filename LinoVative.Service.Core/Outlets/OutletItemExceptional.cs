using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Outlets
{
    public class OutletItemExceptional : AuditableEntity
    {
        public ItemExceptionTypes? Type { get; set; }
        public Guid EntityId { get; set; }
        public Guid OutletId { get; set; }
        public string? Name { get; set; }
    }
}
