using Linovative.Frontend.Shared.Enums;
using MudBlazor;

namespace Linovative.Frontend.Shared.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToMudBlazorColor(this LinovativeColor color) => color switch
        {
           LinovativeColor.Primary => Color.Primary,
           LinovativeColor.Secondary => Color.Secondary,
           LinovativeColor.Info => Color.Info,
           LinovativeColor.Warning => Color.Warning,
           LinovativeColor.Dark => Color.Dark,
           LinovativeColor.Default => Color.Default,
           LinovativeColor.Error => Color.Error,
           LinovativeColor.Success => Color.Success,
           LinovativeColor.Transparent => Color.Transparent,
           LinovativeColor.Inherit => Color.Inherit,
            _=> Color.Primary,
        };
    }
}
