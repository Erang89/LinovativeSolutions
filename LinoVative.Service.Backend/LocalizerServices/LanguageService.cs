using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Core.Interfaces;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace LinoVative.Service.Backend.LocalizerServices
{
    public interface ILanguageService {
        public string RequiredMessage<T>(T dto, Expression<Func<T, object>> expression) where T : class;
        public string EntityNotFound<T>(Guid? id) where T : class;
        public string EntityNotFound<T>(string? id) where T : class;
    }

    public class LanguageService : ILanguageService, IScoopService
    {
        private readonly IStringLocalizer _loc;
        public LanguageService(IStringLocalizer loc)
        {
            _loc = loc;
        }

        public string RequiredMessage<T>(T dto, Expression<Func<T, object>> expression) where T : class 
        {
            var dtoName = typeof(T).Name.Replace("Command", "Dto");
            var propertyName = dto.GetPropertyName(expression);
            var propertyNameLoc = _loc[$"{dtoName}.PropertyName.{propertyName}"];
            return _loc[$"Property.Required", propertyNameLoc];
        }

        public string EntityNotFound<T>(Guid? id) where T : class
        {
            return EntityNotFound<T>($"{id}");
        }

        public string EntityNotFound<T>(string? id) where T : class
        {
            var entityName = typeof(T).Name;
            var entityNameLang = _loc[$"Entity.Name.{entityName}"];
            return _loc["Entity.NotFound", entityNameLang, $"{id}"];
        }

    }
}
