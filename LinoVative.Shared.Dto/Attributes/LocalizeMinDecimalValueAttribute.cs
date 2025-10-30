using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Shared.Dto.Attributes
{
    public class LocalizeMinDecimalValueAttribute : ValidationAttribute
    {
        private decimal MinValue { get; set; }
        public LocalizeMinDecimalValueAttribute(double minValue)
        {
            MinValue = (decimal)minValue;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var val = (decimal)value!;
            if (val < MinValue)
            {
                var localizer = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer))!;

                return new ValidationResult(string.Format(localizer["Validate.MinDecimalValue"], MinValue));
            }

            return ValidationResult.Success;
        }
    }
}
