using Linovative.Frontend.Shared.Pages;

namespace Linovative.Frontend.AccomodationComponents.Bases
{
    public class AccomodationPageBase : LinovativePageBase
    {
        protected override string? LibraryName => "Linovative.Frontend.AccomodationComponents";
        public override string Label(string key) => JsonLocalizer[$"{LocalizerResource}.{key}.Label"];

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
