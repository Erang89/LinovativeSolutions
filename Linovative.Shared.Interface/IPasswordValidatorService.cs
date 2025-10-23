using Microsoft.Extensions.Localization;

namespace Linovative.Shared.Interface
{
    public interface IPasswordValidatorService
    {
        (bool, List<string>) IsStrong(string password, IStringLocalizer localizer);
    }
}
