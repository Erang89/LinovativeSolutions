using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Warehouses
{

    [LocalizerKey(nameof(WarehouseDto))]
    public class WarehouseDto : EntityDtoBase
    {
        [LocalizedRequired]
        public string? Code { get; set; }
        [LocalizedRequired]
        public string? Name { get; set; }
        public string? Address { get; set; }
    }

}
