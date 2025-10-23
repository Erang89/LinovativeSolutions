using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LinoVative.Shared.Dto.Attributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LocalizedRequiredAttribute : RequiredAttribute
    {
        private readonly string? Key;

        public override bool RequiresValidationContext => true;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var localizer = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer))!;
            var className = validationContext.ObjectType.Name;
            var classType = validationContext.ObjectType;

            var propertyName = validationContext.MemberName;
            var localizerKeyAttr = classType.GetCustomAttribute<LocalizerKeyAttribute>(inherit: true);
            var keyFromAttribute = localizerKeyAttr?.Key;

            if(keyFromAttribute is not null)
                keyFromAttribute = $"{keyFromAttribute}.Required.{propertyName}.ErrorMessage";

            var key = keyFromAttribute?? Key ?? $"{className}.Required.{propertyName}.ErrorMessage";
            if (!base.IsValid(value))
            {
                var localizedError = localizer[key];
                return new ValidationResult(localizedError);
            }

            return ValidationResult.Success;
        }
    }
}
