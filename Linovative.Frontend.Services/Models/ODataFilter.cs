namespace Linovative.Frontend.Services.Models
{
    public class ODataFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? OrderFiled { get; set; }
        public string? Options { get; set; }
        private string? _sortDirection;
        public object? FilterPayload { get; set; } = new { searchKeyword = "" };
        public string? SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value is null ? null : value.Contains("Ascending", StringComparison.OrdinalIgnoreCase) ? "asc" : value.Contains("Descending", StringComparison.OrdinalIgnoreCase) ? "desc" : null; }
        }
    }
}
