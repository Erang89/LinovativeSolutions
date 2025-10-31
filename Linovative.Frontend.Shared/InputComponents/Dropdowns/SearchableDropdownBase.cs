using Linovative.Frontend.Services.Models;
using Microsoft.AspNetCore.Components;

namespace Linovative.Frontend.Shared.InputComponents.Dropdowns
{
    public class SearchableDropdownBase<TItem> : ComponentBase
    {
        [Parameter] public TItem? Value { get; set; }
        [Parameter] public bool Immediate { get; set; }
        [Parameter] public bool ReadOnly { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool Required { get; set; }
        [Parameter] public string? RequiredError { get; set; }
        [Parameter] public string? Label { get; set; }
        [Parameter] public string? Text { get; set; }
        [Parameter] public EventCallback<string?> TextChanged { get; set; }
        [Parameter] public EventCallback<TItem?> ValueChanged { get; set; }
        [Parameter] public Func<TItem?, string?> StringConvert { get; set; } = (x) => null;
        [Parameter] public Func<string?, int, Task<Response<List<TItem>>>> SearchFunction { get; set; } = (x, _) => Task.FromResult((new Response<List<TItem>>() { Data = new() }).Result(true));
        [Parameter] public bool IsError { get; set; }
        [Parameter] public string? ErrorMessage { get; set; }
        [Parameter] public int PageSize { get; set; }
        [Parameter] public string? Placeholder { get; set; }
        [Parameter] public bool Clearable { get; set; } = true;
    }
}
