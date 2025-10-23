using Linovative.Shared.Interface.Enums;

namespace Linovative.Shared.Interface
{
    public interface IUniqueFieldValidatorService
    {
        public bool IsValid(EntityTypes? entityType, string fieldName, object fieldValue, IActor actor);
    }
}
