using Linovative.Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Linovative.Frontend.Shared.Pages
{
    public abstract class LinovativePageBase : ComponentBase
    {
        [Inject]
        public IJsonLocalizer JsonLocalizer { get; set; }

        protected abstract string LocalizerResource { get; }
        protected string Lang(string key, params object[] args) 
            => args?.Count() == 0?  
            JsonLocalizer[$"{LocalizerResource}.{key}"] 
            : JsonLocalizer.Format(key, args);

        public string Required(string inputName) => Lang($"{inputName}.Required.ErrorMessage"); 
        public string Label(string inputName) => Lang($"{inputName}.Label"); 
        public string Text(string inputName) => Lang($"{inputName}.Text"); 

        protected override async Task OnInitializedAsync()
        {
            await JsonLocalizer.EnsureLoadedAsync(LocalizerResource); 
            await base.OnInitializedAsync();
        }
    }
}
