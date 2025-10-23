using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LinoVative.Shared.Dto.Attributes
{
    public class UniqueFieldAttribute : ValidationAttribute
    {
        public EntityTypes EntityType { get; set; }
        public string? FieldName { get; set; }

        public UniqueFieldAttribute(EntityTypes entityType) : base()
        {
            EntityType = entityType;
        }

        public UniqueFieldAttribute(EntityTypes entityType, string fieldName) : base() 
        {
            EntityType= entityType;
            FieldName = fieldName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var uniqueValidator = (IUniqueFieldValidatorService)validationContext.GetService(typeof(IUniqueFieldValidatorService))!;
            var localizer = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer))!;
            var actor = (IActor)validationContext.GetService(typeof(IActor))!;

            var fieldName = FieldName??validationContext.MemberName;
            if(value is not null && !uniqueValidator.IsValid(EntityType, fieldName!, value!, actor))
            {
                var classType = validationContext.ObjectType;
                var className = validationContext.ObjectType.Name;

                var localizerKeyAttr = classType.GetCustomAttribute<LocalizerKeyAttribute>(inherit: true);
                var keyFromAttribute = localizerKeyAttr?.Key;
                var propertyName = validationContext.MemberName;

                if (keyFromAttribute is not null)
                    keyFromAttribute = $"{keyFromAttribute}.Unique.{propertyName}.ErrorMessage";

                var key = keyFromAttribute ?? $"{className}.Unique.{propertyName}.ErrorMessage";

                var localizedError = localizer[key, value!];
                return new ValidationResult(localizedError);
            }

            return ValidationResult.Success;
        }
    }
}
