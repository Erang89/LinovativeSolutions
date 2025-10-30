using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Accountings
{
    [LocalizerKey(nameof(COAGroupDto))]
    public class COAGroupDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField(EntityTypes.COAGroup)]
        public string? Name { get; set; }
        public COATypes? Type { get; set; }
    }
}