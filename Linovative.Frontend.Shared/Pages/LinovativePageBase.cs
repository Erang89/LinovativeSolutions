using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Linovative.Frontend.Shared.Pages
{
    public abstract class LinovativePageBase : ComponentBase
    {
        [Inject]
        public IJsonLocalizer JsonLocalizer { get; set; }

        protected virtual string? LocalizerResource { get; }
        protected virtual string? LibraryName { get; }
        protected string Lang(string key, params object[] args) 
            => args?.Count() == 0?  
            JsonLocalizer[$"{LocalizerResource}.{key}"] 
            : JsonLocalizer.Format(key, args);

        public string Required(string inputName) => Lang($"{inputName}.Required.ErrorMessage"); 
        public virtual string Label(string inputName) => Lang($"{inputName}.Label"); 
        public string Text(string inputName) => Lang($"{inputName}.Text"); 

        protected override async Task OnInitializedAsync()
        {
            if(LocalizerResource is not null) 
                await JsonLocalizer.EnsureLoadedAsync(LocalizerResource, LibraryName); 

            await base.OnInitializedAsync();
        }

        protected string ADD => JsonLocalizer.Global("Label.Add");
        protected string UPDATE => JsonLocalizer.Global("Label.Update");
        protected string YES => JsonLocalizer.Global("Dialog.Yes");
        protected string CANCEL => JsonLocalizer.Global("Dialog.Cancel");
        protected string NO => JsonLocalizer.Global("Dialog.No");
        protected string DELETE => JsonLocalizer.Global("Dialog.Delete");

        protected virtual string EntityName => "Data";
        protected virtual string EntityPluralName => "Data";

        protected string CreatedSuccessMessage => string.Format(JsonLocalizer.Global("Dialog.CreatedSuccessMessage"), EntityName);
        protected string UpdateSuccessMessage => string.Format(JsonLocalizer.Global("Dialog.UpdateSuccessMessage"), EntityName);
        protected string DeleteConfirmation => JsonLocalizer.Global("Dialog.DeleteConfirmation");
        protected string DeleteSuccessMessage => JsonLocalizer.Global("Dialog.DeleteSuccessMessage");

    }
}
