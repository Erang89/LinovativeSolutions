using Linovative.Shared.Interface;

namespace LinoVative.Shared.Dto
{
    public abstract class EntityDtoBase : IEntityId, IDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
