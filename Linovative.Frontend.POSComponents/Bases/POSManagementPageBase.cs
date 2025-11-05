using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Shared.Pages;

namespace Linovative.Frontend.POSComponents.Bases
{
    public class POSManagementPageBase : LinovativePageBase
    {
        protected override string? LibraryName => "Linovative.Frontend.POSComponents";
        public override string Label(string key) => JsonLocalizer[$"{LocalizerResource}.{key}.Label"];
    }

    
}
