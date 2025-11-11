using Linovative.Frontend.Shared.Pages;

namespace Linovative.Frontend.SettingComponent.Bases
{
    public class SettingPageBase : LinovativePageBase
    {
        protected override string? LibraryName => "Linovative.Frontend.SettingComponent";
        public override string Label(string key) => JsonLocalizer[$"{LocalizerResource}.{key}.Label"];

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
