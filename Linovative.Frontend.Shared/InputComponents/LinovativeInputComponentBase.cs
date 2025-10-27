using Linovative.Frontend.Shared.Enums;
using Microsoft.AspNetCore.Components;
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
        public string? RequiredMessage { get; set; } = "This field is required";

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
        public bool Error { get; set; }
    }
}
