using Linovative.Frontend.Shared.Pages;

namespace Linovative.Frontend.POSCashierComponent.Bases
{
    public class PosCashierPageBase : LinovativePageBase
    {
        protected override string? LibraryName => "Linovative.Frontend.POSCashierComponent";
        public override string Label(string key) => JsonLocalizer[$"{LocalizerResource}.{key}.Label"];

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
