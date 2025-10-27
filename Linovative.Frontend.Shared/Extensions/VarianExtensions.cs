using Linovative.Frontend.Shared.Enums;
using MudBlazor;

namespace Linovative.Frontend.Shared.Extensions
{
    public static class VarianExtensions
    {
        public static Variant ToMudBlazorVariant(this LinovativeInputComponentVariant variant) => variant switch
        {
           Enums.LinovativeInputComponentVariant.Filled => MudBlazor.Variant.Filled,
            Enums.LinovativeInputComponentVariant.Outline => MudBlazor.Variant.Outlined,
            _ => MudBlazor.Variant.Text
        };
    }
}
