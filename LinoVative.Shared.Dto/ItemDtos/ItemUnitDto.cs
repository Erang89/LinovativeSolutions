using Linovative.Shared.Interface;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.ItemDtos
{
    public class ItemUnitDto : IEntityId
    {
        public Guid Id { get; set; }
        [InputRequired]
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
