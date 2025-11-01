using Linovative.Frontend.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace Linovative.Frontend.Shared.InputComponents
{
    public class LinovativeButtonBase : ComponentBase
    {
        [Parameter] public string? Text { get; set; }
        [Parameter] public bool FullWidth { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public LinovativeSize Size { get; set; } = LinovativeSize.Medium;
        [Parameter] public LinovativeColor Color { get; set; }
        [Parameter] public LinovativeInputComponentVariant Variant { get; set; }
        [Parameter] public EventCallback OnClicked { get; set; }
        
    }
}
