using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

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
