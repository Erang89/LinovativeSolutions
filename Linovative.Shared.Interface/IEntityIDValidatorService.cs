using Linovative.Shared.Interface.Enums;

namespace Linovative.Shared.Interface
{
    public interface IEntityIDValidatorService
    {
        public bool IsValid(EntityTypes? entityType, Guid id, IActor actor);
    }
}
