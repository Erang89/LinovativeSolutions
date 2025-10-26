using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.Enums
{
    public enum FilterOperator
    {
        [EnumDisplayName("equals")]
        Equals = 1,
        [EnumDisplayName("not equals")]
        NotEquals = 2,
        [EnumDisplayName("contains")]
        Contains = 3,
        [EnumDisplayName("not contains")]
        NotContains = 4,
        [EnumDisplayName("starts with")]
        StartsWith = 5,
        [EnumDisplayName("ends with")]
        EndsWith = 6,
        [EnumDisplayName("greater than")]
        GreaterThan = 7,
        [EnumDisplayName("greater than or equal")]
        GreaterThanOrEqual = 8,
        [EnumDisplayName("less than")]
        LessThan = 9,
        [EnumDisplayName("less than or equal")]
        LessThanOrEqual = 10,
        [EnumDisplayName("is empty")]
        IsNull = 11,
        [EnumDisplayName("is not empty")]
        IsNotNull = 12,
        [EnumDisplayName("in")]
        In = 13,
        [EnumDisplayName("not in")]
        NotIn = 14
    }
}
