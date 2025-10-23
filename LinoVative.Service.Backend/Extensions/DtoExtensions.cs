using System.Linq.Expressions;

namespace LinoVative.Service.Backend.Extensions
{
    public static class DtoExtensions
    {
        public static string GetPropertyName<T>(this T dto, Expression<Func<T, object>> expression) where T : class
        {
            if (expression.Body is MemberExpression member)
            {
                return member.Member.Name;
            }

            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberExpr)
            {
                return memberExpr.Member.Name;
            }

            throw new ArgumentException("Invalid expression");
        }


        public static string GetPropertyName<T>(Expression<Func<T, object>> expression) where T : class
        {
            if (expression.Body is MemberExpression member)
            {
                return member.Member.Name;
            }

            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberExpr)
            {
                return memberExpr.Member.Name;
            }

            throw new ArgumentException("Invalid expression");
        }
    }
}

