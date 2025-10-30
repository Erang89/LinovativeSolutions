using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;


namespace LinoVative.Shared.Dto.Attributes
{
    public class LocalizedPhoneAttribute : ValidationAttribute
    {
        private readonly PhoneAttribute _phone = new PhoneAttribute();
        
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (!_phone.IsValid(value))
            {
                var localizer = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer))!;
                var msg = localizer?["Validate.PhoneInvalid"] ?? "Invalid phone number format";
                return new ValidationResult(msg);
            }

            return ValidationResult.Success;
        }
    }
}
