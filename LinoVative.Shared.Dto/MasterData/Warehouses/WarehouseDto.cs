using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Warehouses
{

    [LocalizerKey(nameof(WarehouseDto))]
    public class WarehouseDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField(EntityTypes.Warehouse)]
        public string? Code { get; set; }
        [LocalizedRequired, UniqueField(EntityTypes.Warehouse)]
        public string? Name { get; set; }
        public string? Address { get; set; }
    }

}
