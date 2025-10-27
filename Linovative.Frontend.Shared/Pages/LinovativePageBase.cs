using Linovative.Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Linovative.Frontend.Shared.Pages
{
    public abstract class LinovativePageBase : ComponentBase
    {
        [Inject]
        public IJsonLocalizer JsonLocalizer { get; set; }

        protected abstract string LocalizerKey { get; }
        protected string Lang(string key, params object[] args) 
            => args?.Count() == 0?  
            JsonLocalizer[$"{LocalizerKey}.key"] 
            : JsonLocalizer.Format(key, args);

        protected override async Task OnInitializedAsync()
        {
            await JsonLocalizer.EnsureLoadedAsync(LocalizerKey); 
            await base.OnInitializedAsync();
        }
    }
}
