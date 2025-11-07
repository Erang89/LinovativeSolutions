using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Shared.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq.Expressions;

namespace Linovative.Frontend.Shared.InputComponents
{
    public class LinovativeInputComponentBase<T> : ComponentBase
    {
        [Parameter]
        public T? Value { get; set; }

        [Parameter] public EventCallback<T?> ValueChanged { get; set; }

        [Parameter]
        public bool IsRequired { get; set; }

        [Parameter]
        public string? RequiredMessage { get; set; }

        [Parameter]
        public string? ErrorKey { get; set; }

        [Parameter]
        public IDictionary<string, List<string>> Errors { get; set; } =  new Dictionary<string, List<string>>();

        [Parameter]
        public string? ErrorMessage { get; set; }

        [Parameter]
        public Expression<Func<string>>? ValueExpression { get; set; }

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public LinovativeInputComponentVariant Variant { get; set; }

        [Parameter]
        public string Class { get; set; } = string.Empty;

        [Parameter]
        public bool ReadOnly { get; set; }


        [Parameter]
        public bool Disabled { get; set; }


        [Parameter]
        public string? Format { get; set; }

        [Parameter]
        public string? Style { get; set; }

        [Parameter]
        public bool Error { get; set; }

        [Parameter]
        public bool Immediate { get; set; }

        [Parameter]
        public bool Clearable { get; set; }

        [Parameter] public string? LocalizerKey { get; set; }
        [Parameter] public string? LocalizerResource { get; set; }

        [Inject] public IJsonLocalizer JsonLocalizer { get; set; }
        [Parameter] public InputType InputType { get; set; } = InputType.Text;
    }
}
