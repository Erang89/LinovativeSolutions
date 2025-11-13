using LinoVative.Shared.Dto.Enums;

namespace Linovative.Frontend.Shared.InputComponents.FilterComponents
{
    public class DataGridFilterModel
    {
        public DataGridFilterModel()
        {

        }

        public DataGridFilterModel(string columnName)
        {
            Column = columnName;
        }
        public enum PropertyType
        {
            Decimal,
            Integer,
            String,
            DateTime,
            DateOnly,
            Boolean,
            Guid,
            ListOfGuid,
            Enum,
            ListOfEnum
        }
        public string Column { get; set; } = string.Empty;
        public object? Value { get; set; }
        public FilterOperator? Operator { get; set; }
        public PropertyType? Type { get; set; }
    }
}
