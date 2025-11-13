using Linovative.Frontend.Shared.InputComponents.FilterComponents;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.Enums;

namespace Linovative.Frontend.Shared.Extensions
{
    public static class DataGridFilterModelExtensions
    {
        public static List<FilterCondition> ToFilterCondition(this List<DataGridFilterModel> filterModels)
        {
            var result = new List<FilterCondition>();
            foreach (var model in filterModels)
            {
                var fc = new FilterCondition()
                {
                    Field = model.Column,
                    Value = GetValue(model),
                    Operator = model.Operator ?? FilterOperator.Equals
                };
                result.Add(fc);
            }

            return result;
        }

        static string? GetValue(DataGridFilterModel model)
        {
            if (model.Value is null) return null;

            if (model.Type == DataGridFilterModel.PropertyType.ListOfGuid)
            {
                var values = (List<Guid>)model.Value;
                return string.Join(",", values.Select(x => x.ToString()));
            }

            var value = (string?)(model.Value.ToString());
            return value;
        }
    }
}
