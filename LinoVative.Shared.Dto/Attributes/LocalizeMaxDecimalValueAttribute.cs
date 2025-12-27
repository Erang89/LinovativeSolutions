using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Shared.Dto.Attributes
{
    public class LocalizeMaxDecimalValueAttribute : ValidationAttribute
    {
        private decimal MaxValue { get; set; }
        public LocalizeMaxDecimalValueAttribute(double minValue)
        {
            MaxValue = (decimal)minValue;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null) return ValidationResult.Success;

            var val = (decimal)value!;
            if (val > MaxValue)
            {
                var localizer = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer))!;

                return new ValidationResult(string.Format(localizer["Validate.MaxDecimalValue"], MaxValue));
            }

            return ValidationResult.Success;
        }
    }
}
