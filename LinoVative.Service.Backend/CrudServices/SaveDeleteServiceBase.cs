using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace LinoVative.Service.Backend.CrudServices
{
    public abstract class SaveDeleteServiceBase<T, TRequest> : QueryServiceBase<T> where T : class, IEntityId where TRequest : class, IEntityId
    {
        protected IStringLocalizer _loc;

        protected SaveDeleteServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache,  IStringLocalizer log) : base(dbContext, actor, mapper, appCache)
        {
            _loc = log;
        }

        protected string EntityName => _loc[$"Entity.Name.{typeof(T).Name}"];

        // ==============================  Delete ==============================
        protected virtual async Task<Result> SaveDelete(TRequest request, CancellationToken token = default)
        {
            var result = Result.OK();
            var entity = await GetById(request.Id);
            
            if (entity is null)
                AddError(result, x => x.Id!, _loc["Entity.IdNotFound", EntityName, request.Id]);

            if (!result) return result;

            return await SaveDelete(request, new List<T>() { entity! }, token);
        }

        protected virtual Task<Result> SaveDelete(TRequest request,  List<Guid> ids, CancellationToken token = default)
        {
            var entities = GetAll().Where(x => ids.Contains(x.Id)).ToList();
            return SaveDelete(request, entities, token);
        }

        protected virtual async Task<Result> SaveDelete(TRequest request,  List<T> entities, CancellationToken token = default)
        {
            var validate = await ValidateSaveDelete(request, entities, token);
            if (!validate) return validate;

            foreach(var entity in entities)
            {
                if (entity is IDeleteableEntity entityToDelete)
                    entityToDelete.Delete(_actor);
                else
                    _dbSet.Remove(entity);
            }

            await BeforeSaveDelete(request, entities, token);

            return await _dbContext.SaveAsync(_actor);
        }

        protected virtual Task<Result> SaveDelete(TRequest request,  T entity, CancellationToken token = default)
        {
            return SaveDelete(request, new List<T>() { entity}, token);
        }


        protected virtual Task BeforeSaveDelete(TRequest request, List<T> entities, CancellationToken token = default) => Task.CompletedTask;
        protected virtual async Task<Result> ValidateSaveDelete(TRequest request, List<T> entities, CancellationToken token = default)
        {
            var entityIds = entities.Select(x => x.Id).ToList();
            var anyFalseId = GetAll().Where(x => entityIds.Contains(x.Id)).Select(x => x.Id).ToList().Any(x => !entityIds.Contains(x));

            var result = Result.OK();

            if (anyFalseId)
                AddError(result, x => x.Id!, _loc["Entity.IdNotFound", EntityName, request.Id]);

            await Task.CompletedTask;
            return Result.OK();
        }


        protected string Prop(Expression<Func<TRequest, object>> expresion) => DtoExtensions.GetPropertyName(expresion);

        protected void AddError(Result result, Expression<Func<TRequest, object>> expresion, string message) =>
        result.AddInvalidProperty(Prop(expresion), message);

        protected void AddError(Result result, Expression<Func<TRequest, object>> expresion, object obj) =>
            result.AddInvalidProperty(Prop(expresion), obj);

    }
}
