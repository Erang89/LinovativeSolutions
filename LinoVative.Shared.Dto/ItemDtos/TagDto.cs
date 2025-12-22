using LinoVative.Shared.Dto.Attributes;
using Linovative.Shared.Interface.Enums;

namespace LinoVative.Shared.Dto.ItemDtos
{

    [LocalizerKey(nameof(TagDto))]
    public class TagDto : EntityDtoBase
    {
        [LocalizedRequired, UniqueField(EntityTypes.Tag)]
        public string? Name { get; set; }
    }
}
