using Linovative.Shared.Interface.Enums;

namespace LinoVative.Shared.Dto.MasterData.Outlets
{
    public class OutletItemExceptional
    {
        public ItemExceptionTypes? Type { get; set; }
        public Guid EntityId { get; set; }
        public Guid OutletId { get; set; }
        public string? Name { get; set; }
    }
}
