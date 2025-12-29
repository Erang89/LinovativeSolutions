using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LinoVative.Shared.Dto.Attributes
{
    public class EntityIDAttribute : ValidationAttribute
    {
        public EntityTypes? EntityType { get; set; }
        public EntityIDAttribute(EntityTypes entityType)
        {
            EntityType = entityType;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is null) return ValidationResult.Success;


            var classType = validationContext.ObjectType;
            var ignoreAttribute = classType.GetCustomAttribute<IgnoreEntityIDValidationAttribute>(inherit: true);
            var classProperties = ignoreAttribute?.IgnoreList;
            var anyProperties = classProperties is not null && classProperties.Any();
            if (classProperties is null || classProperties.Count() == 0)
                return ValidationResult.Success;

            var className = classType.Name;
            var propertyName = validationContext.MemberName;
            if(anyProperties && classProperties.Any(x => x.ClassName == className && x.PropertyName == propertyName))
                return ValidationResult.Success;

            var idValidator = (IEntityIDValidatorService)validationContext.GetService(typeof(IEntityIDValidatorService))!;
            var localizer = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer))!;
            var actor = (IActor)validationContext.GetService(typeof(IActor))!;

            if (value is not null && !idValidator.IsValid(EntityType, (Guid)value!, actor))
            {
                var entityName = localizer[$"Entity.Name.{EntityType}"];
                var localizedError = localizer["Entity.IdNotFound", entityName, value!];
                return new ValidationResult(localizedError);
            }

            return ValidationResult.Success;
        }
    }
}
