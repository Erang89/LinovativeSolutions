using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Commons;
using LinoVative.Service.Core.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Service.Backend.CrudServices
{
    public abstract class SaveNewServiceBase<T, TRequest> : QueryServiceBase<T> where T : class, IEntityId
    {
        protected IStringLocalizer _localizer;
        protected SaveNewServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) 
            : base(dbContext, actor, mapper, appCache)
        { 
            _localizer = localizer;
        }


        // ============================== Maping Entity ==============================
        protected virtual async Task<T> OnMapping(TRequest request)
        {
            await Task.CompletedTask;
            return _mapper.Map<T>(request!);
        }

        protected virtual async Task<Result> SaveNew(TRequest request, CancellationToken token)
        {
            var validate = await Validate(request, token);
            if (!validate)
                return validate;
            
            var creatingResult = await OnCreatingEntity(request, token);

            _dbSet.AddRange(creatingResult);

            var result = await _dbContext.SaveAsync(_actor, token);
            if(result) return Result.OK(creatingResult);

            return result;
        }
        

        protected virtual async Task<List<T>> OnCreatingEntity(TRequest request, CancellationToken token = default)
        {
            T entity = await OnMapping(request!);

            if (typeof(IsEntityManageByClinet).IsAssignableFrom(typeof(T)))
            {
                ((IsEntityManageByClinet)entity).ClientId = _actor.ClientId;
            }

            return new() { entity };
        }


        protected virtual async Task<Result> Validate(TRequest request, CancellationToken token)
        {
            var validate = await ValidateRequiredPropery(request, token);
            if (!validate) return validate;


            await Task.CompletedTask;
            return Result.OK();
        }

        protected virtual async Task<Result> ValidateRequiredPropery(TRequest request, CancellationToken token)
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

                var propertyNameLoc = _localizer[$"{dtoName}.PropertyName.{propertyName}"];
                var message = _localizer[$"Property.Required", propertyNameLoc];

                if (errors.ContainsKey(propertyName))
                {
                    var errStrings = errors[propertyName];
                    if(!errStrings.Contains(message))
                        errStrings.Add(message);

                    errors[propertyName] = errStrings;
                    continue;
                }

                errors[prop.Name] = new List<string>() { message};
            }

            if (errors.Count > 0)
            {
                var errorDetails = new Dictionary<string, object>();
                foreach (var error in errors)
                    errorDetails.Add(error.Key, error.Value);

                return Result.Failed(string.Empty, null, errorDetails);
            }

            await Task.CompletedTask;
            return Result.OK();

        }
    }
}
