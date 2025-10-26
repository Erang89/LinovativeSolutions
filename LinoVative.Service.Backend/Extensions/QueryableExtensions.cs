using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.Enums;
using System.Linq.Expressions;
using System.Reflection;

namespace LinoVative.Service.Backend.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable WhereEquals(this IQueryable source, string propertyName, object? value)
        {
            var elementType = source.ElementType;             // e.g. ItemUnit
            var parameter = Expression.Parameter(elementType, "x");
            var property = Expression.PropertyOrField(parameter, propertyName);

            // convert value to property type
            var constant = Expression.Constant(value, property.Type);

            // x.Property == value
            var body = Expression.Equal(property, constant);
            var lambda = Expression.Lambda(body, parameter);

            var whereMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == nameof(Queryable.Where)
                         && m.GetParameters().Length == 2)
                .MakeGenericMethod(elementType);

            return (IQueryable)whereMethod.Invoke(null, new object[] { source, lambda })!;
        }

        public static bool AnyDynamic(this IQueryable source)
        {
            var elementType = source.ElementType;

            var anyMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == nameof(Queryable.Any) && m.GetParameters().Length == 1)
                .MakeGenericMethod(elementType);

            return (bool)anyMethod.Invoke(null, new object[] { source })!;
        }

        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> source, List<FilterCondition> filters)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? body = null;

            foreach (var filter in filters)
            {

                if (filter.Operator is FilterOperator.Contains or FilterOperator.NotContains or FilterOperator.Equals or FilterOperator.NotEquals && string.IsNullOrEmpty(filter.Value))
                    continue;

                Expression propertyExpression = parameter;
                Type propertyType = typeof(T);

                foreach (var propName in filter.Field.Split('.'))
                {
                    var propertyInfo = propertyType.GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null)
                    {
                        propertyExpression = null!;
                        break;
                    }

                    propertyExpression = Expression.Property(propertyExpression, propertyInfo);
                    propertyType = propertyInfo.PropertyType;
                }

                if (propertyExpression == null)
                    continue;

                Expression comparison;

                if (filter.Operator == FilterOperator.IsNull)
                {
                    comparison = Expression.Equal(propertyExpression, Expression.Constant(null));
                }
                else if (filter.Operator == FilterOperator.IsNotNull)
                {
                    comparison = Expression.NotEqual(propertyExpression, Expression.Constant(null));
                }
                else if (filter.Operator == FilterOperator.In || filter.Operator == FilterOperator.NotIn)
                {
                    var elementType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                    var rawValues = string.IsNullOrWhiteSpace(filter.Value)
                        ? new List<string>()
                        : filter.Value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim())
                                      .ToList();

                    var convertedValues = Array.CreateInstance(elementType, rawValues.Count);
                    for (int i = 0; i < rawValues.Count; i++)
                    {
                        object? val;
                        try
                        {
                            if (elementType == typeof(Guid))
                                val = Guid.Parse(rawValues[i]);
                            else
                                val = Convert.ChangeType(rawValues[i], elementType);
                        }
                        catch
                        {
                            continue; // Skip bad value
                        }
                        convertedValues.SetValue(val, i);
                    }

                    var containsMethod = typeof(Enumerable)
                        .GetMethods()
                        .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                        .MakeGenericMethod(elementType);

                    var collection = Expression.Constant(convertedValues);

                    // Convert property expression to match non-nullable element type
                    Expression valueExpression = propertyExpression;
                    if (Nullable.GetUnderlyingType(propertyType) != null)
                    {
                        valueExpression = Expression.Convert(propertyExpression, elementType);
                    }

                    var containsExpr = Expression.Call(containsMethod, collection, valueExpression);

                    comparison = filter.Operator == FilterOperator.In
                        ? containsExpr
                        : Expression.Not(containsExpr);
                }
                else
                {
                    object? typedValue;
                    try
                    {
                        var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                        if (targetType == typeof(Guid))
                            typedValue = Guid.Parse(filter.Value!);
                        else
                            typedValue = Convert.ChangeType(filter.Value, targetType);
                    }
                    catch
                    {
                        continue; // Skip invalid value
                    }

                    var constant = Expression.Constant(typedValue, propertyType);

                    switch (filter.Operator)
                    {
                        case FilterOperator.Equals:
                            comparison = Expression.Equal(propertyExpression, constant);
                            break;
                        case FilterOperator.NotEquals:
                            comparison = Expression.NotEqual(propertyExpression, constant);
                            break;
                        case FilterOperator.GreaterThan:
                            comparison = Expression.GreaterThan(propertyExpression, constant);
                            break;
                        case FilterOperator.GreaterThanOrEqual:
                            comparison = Expression.GreaterThanOrEqual(propertyExpression, constant);
                            break;
                        case FilterOperator.LessThan:
                            comparison = Expression.LessThan(propertyExpression, constant);
                            break;
                        case FilterOperator.LessThanOrEqual:
                            comparison = Expression.LessThanOrEqual(propertyExpression, constant);
                            break;
                        case FilterOperator.Contains:
                            comparison = Expression.Call(propertyExpression, nameof(string.Contains), null, constant);
                            break;
                        case FilterOperator.NotContains:
                            comparison = Expression.Not(Expression.Call(propertyExpression, nameof(string.Contains), null, constant));
                            break;
                        case FilterOperator.StartsWith:
                            comparison = Expression.Call(propertyExpression, nameof(string.StartsWith), null, constant);
                            break;
                        case FilterOperator.EndsWith:
                            comparison = Expression.Call(propertyExpression, nameof(string.EndsWith), null, constant);
                            break;
                        case FilterOperator.IsNull:
                            comparison = Expression.Equal(
                                   propertyExpression,
                                   Expression.Constant(null, propertyExpression.Type)
                               );
                            break;
                        case FilterOperator.IsNotNull:
                            comparison = Expression.Not(Expression.Equal(
                                   propertyExpression,
                                   Expression.Constant(null, propertyExpression.Type)
                               ));
                            break;
                        default:
                            throw new NotSupportedException($"Operator {filter.Operator} not supported");
                    }
                }

                body = body == null ? comparison : Expression.AndAlso(body, comparison);
            }

            if (body == null)
                return source;

            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
            return source.Where(lambda);
        }

    }
}
