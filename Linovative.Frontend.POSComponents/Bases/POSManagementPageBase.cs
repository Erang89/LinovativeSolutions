using Linovative.Frontend.Shared.Pages;

namespace Linovative.Frontend.POSComponents.Bases
{
    public class POSManagementPageBase : LinovativePageBase
    {
        protected override string? LibraryName => "Linovative.Frontend.POSComponents";
        public override string Label(string key) => JsonLocalizer[$"{LocalizerResource}.{key}.Label"];
        public string ButtonText(string key) => JsonLocalizer[$"{LocalizerResource}.Button.{key}.Text"];
        public string ColumHeaderText(string key) => JsonLocalizer[$"{LocalizerResource}.Column.{key}.Text"];

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }

   
}
