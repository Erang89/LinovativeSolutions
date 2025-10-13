using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace LinoVative.Service.Backend.Helpers
{
    public static class AttributeHelpers
    {
        public static string DisplayName<T, TProp>(Expression<Func<T, TProp>> expression)
        {
            if (expression.Body is MemberExpression memberExpr)
            {
                var propInfo = memberExpr.Member as PropertyInfo;
                if (propInfo == null)
                    throw new ArgumentException("Expression is not a property");

                var attr = propInfo.GetCustomAttribute<DisplayNameAttribute>();
                return attr?.DisplayName ?? propInfo.Name;
            }

            throw new ArgumentException("Invalid expression");
        }



        public static string? GetDisplayName<T>()
        {
            var type = typeof(T);
            var attr = type.GetCustomAttribute<DisplayNameAttribute>();
            return attr?.DisplayName;
        }

    }
}
