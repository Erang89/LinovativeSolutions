using Linovative.Frontend.Shared.Pages;

namespace Linovative.Frontend.InventoryComponents.Bases
{
    public class InventoryPageBase : LinovativePageBase
    {
        protected override string? LibraryName => "Linovative.Frontend.InventoryComponents";
        public override string Label(string key) => JsonLocalizer[$"{LocalizerResource}.{key}.Label"];

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
