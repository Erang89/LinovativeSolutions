using Linovative.Frontend.Shared.Pages;

namespace Linovative.Frontend.AccountingComponents.Bases
{
    public class AccountingPageBase : LinovativePageBase
    {
        protected override string? LibraryName => "Linovative.Frontend.AccountingComponents";
        public override string Label(string key) => JsonLocalizer[$"{LocalizerResource}.{key}.Label"];

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
