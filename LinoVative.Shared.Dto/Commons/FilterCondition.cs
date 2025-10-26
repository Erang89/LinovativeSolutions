using LinoVative.Shared.Dto.Enums;

namespace LinoVative.Shared.Dto.Commons
{
    public class FilterCondition
    {
        public string Field { get; set; } = string.Empty;
        public string? Value { get; set; }
        public FilterOperator Operator { get; set; } = FilterOperator.Equals;
    }
}
