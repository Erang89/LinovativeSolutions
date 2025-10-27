using Linovative.Frontend.Shared.Enums;
using MudBlazor;

namespace Linovative.Frontend.Shared.Extensions
{
    public static class SizeExtensions
    {
        public static Size ToMudBlazorSize(this LinovativeSize size) => size switch
        {
           LinovativeSize.Small => Size.Small,
           LinovativeSize.Medium => Size.Medium,
           LinovativeSize.Large => Size.Large,
            _=> Size.Medium,
        };
    }
}
