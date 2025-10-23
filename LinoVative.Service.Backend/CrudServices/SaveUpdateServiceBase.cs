using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Backend.LocalizerServices;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto;
using Mapster.Utils;
using MapsterMapper;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace LinoVative.Service.Backend.CrudServices
{
    public abstract class SaveUpdateServiceBase<T, TRequest> : QueryServiceBase<T> where T : class, IEntityId where TRequest : class, IEntityId
    {
        protected readonly IStringLocalizer _localizer;
        protected abstract string LocalizerPrefix { get; }
        protected SaveUpdateServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer stringLocalizer) : base(dbContext, actor, mapper, appCache)
        { 
            _localizer = stringLocalizer;
        }

        protected string EntityName => _localizer[$"Entity.Name.{typeof(T).Name}"];

        // ============================== Maping Entity ==============================
        protected virtual async Task<T> OnMapping(TRequest request)
        {
            await Task.CompletedTask;
            return _mapper.Map<T>(request!);
        }

        protected virtual async Task<T> OnMapping(TRequest request, T entity)
        {
            await Task.CompletedTask;
            return _mapper.Map(request!, entity);
        }


        // ============================== Update ==============================
        protected virtual async Task<Result> SaveUpdate(TRequest request, CancellationToken token)
        {
            var validate = await ValidateSaveUpdate(request, token);
            if (!validate)
                return validate;

            T entity = (await GetById(request.Id))!;

            entity = await OnMapping(request, entity);

            await BeforeSaveUpdate(request, entity, token);
            
            if (typeof(IsEntityManageByCompany).IsAssignableFrom(typeof(T)))
            {
                ((IsEntityManageByCompany)entity!).CompanyId = _actor.CompanyId;
            }

            return await _dbContext.SaveAsync(_actor, token);
        }
        protected virtual Task BeforeSaveUpdate(TRequest request, T entity, CancellationToken token) => Task.CompletedTask;
        protected virtual async Task<Result> ValidateSaveUpdate(TRequest request, CancellationToken token)
        {
            var validate = Result.OK();
            
            var entity = await GetById(request.Id);
            if (entity is null)
                AddError(validate, x => x.Id!, _localizer["Entity.IdNotFound", EntityName, request.Id]);

            return validate;
        }


        protected string Prop(Expression<Func<TRequest, object>> expresion) => DtoExtensions.GetPropertyName(expresion);

        protected void AddError(Result result, Expression<Func<TRequest, object>> expresion, string message) =>
            result.AddInvalidProperty(Prop(expresion), message);

        protected void AddError(Result result, Expression<Func<TRequest, object>> expresion, object obj) =>
            result.AddInvalidProperty(Prop(expresion), obj);


    }
}
