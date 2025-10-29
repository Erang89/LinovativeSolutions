using Linovative.Shared.Interface.Enums;

namespace Linovative.Shared.Interface
{
    public interface IUniqueFieldValidatorService
    {
        public bool IsValid(EntityTypes? entityType, Guid entityId, string fieldName, object fieldValue, IActor actor, object dto);
    }
}
