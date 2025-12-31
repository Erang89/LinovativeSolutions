using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Shared.Dto.Attributes
{
    public class LocalizedEmailAttribute : ValidationAttribute
    {
        private readonly EmailAddressAttribute _validator = new EmailAddressAttribute();

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var email = (string?)value;

            if (!string.IsNullOrWhiteSpace(email) && !_validator.IsValid(value))
            {
                var localizer = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer))!;
                var msg = localizer?["Validate.EmailInvalid", value!] ?? "Invalid email format";
                return new ValidationResult(msg);
            }

            return ValidationResult.Success;
        }
    }
}
