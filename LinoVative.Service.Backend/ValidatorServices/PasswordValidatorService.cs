using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Helpers;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.ValidatorServices
{
    public class PasswordValidatorService : IPasswordValidatorService
    {
        public (bool, List<string>) IsStrong(string password, IStringLocalizer localizer)
        {
            return PasswordHelper.IsPasswordStrong(password, localizer);
        }
    }
}
