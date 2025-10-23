using System.Linq.Expressions;

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
    }
}
