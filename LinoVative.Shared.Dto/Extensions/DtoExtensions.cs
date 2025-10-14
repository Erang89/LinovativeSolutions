using LinoVative.Shared.Dto.Attributes;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinoVative.Shared.Dto.Extensions
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

        public static Result ValidateRequiredPropery<TRequest>(this TRequest request, IStringLocalizer localizer) where TRequest : class
        {
            var errors = new Dictionary<string, List<string>>();
            var props = typeof(TRequest).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var prop in props)
            {
                // only properties with [Required]
                var required = prop.GetCustomAttribute<InputRequiredAttribute>(inherit: true);
                if (required is null) continue;

                // current value
                var value = prop.GetValue(request);

                // missing?
                bool isMissing =
                    value is null
                    || (value is string s && string.IsNullOrWhiteSpace(s))
                    || (prop.PropertyType == typeof(Guid) && (Guid)value == Guid.Empty)
                    || (prop.PropertyType == typeof(Guid?) && (!((Guid?)value).HasValue || ((Guid?)value).Value == Guid.Empty));

                if (!isMissing) continue;


                var propertyName = prop.Name;
                var dtoName = typeof(TRequest).Name.Replace("ServiceCommand", "Dto");
                dtoName = dtoName.Replace("Command", "Dto");

                var propertyNameLoc = localizer[$"{dtoName}.PropertyName.{propertyName}"];
                var message = localizer[$"Property.Required", propertyNameLoc];

                if (errors.ContainsKey(propertyName))
                {
                    var errStrings = errors[propertyName];
                    if (!errStrings.Contains(message))
                        errStrings.Add(message);

                    errors[propertyName] = errStrings;
                    continue;
                }

                errors[prop.Name] = new List<string>() { message };
            }

            if (errors.Count > 0)
            {
                var errorDetails = new Dictionary<string, object>();
                foreach (var error in errors)
                    errorDetails.Add(error.Key, error.Value);

                return Result.Failed(string.Empty, null, errorDetails);
            }

            return Result.OK();


        }
    }
}
