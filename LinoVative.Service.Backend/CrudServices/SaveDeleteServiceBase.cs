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
    public abstract class SaveDeleteServiceBase<T, TRequest> : QueryServiceBase<T> where T : class, IEntityId where TRequest : class
    {
        protected IStringLocalizer _loc;
        private List<T>? _entities = null;

        protected SaveDeleteServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache,  IStringLocalizer log) : base(dbContext, actor, mapper, appCache)
        {
            _loc = log;
        }

        protected string EntityName => _loc[$"Entity.Name.{typeof(T).Name}"];

        // ==============================  Delete ==============================
        
        protected virtual List<T> OnMapping(TRequest request)
        {
            if (_entities != null)
                return _entities!;

            if (request is IEntityId entity)
            {
                _entities = GetAll().Where(x => x.Id == entity.Id).ToList();
                return _entities;
            }

            if(request is IBulkDeleteDto dto)
            {
                _entities =  GetAll().Where(x => dto.Ids.Contains(x.Id)).ToList();
                return _entities;
            }

            throw new NotImplementedException();
        }

        public virtual async Task<Result> Handle(TRequest request, CancellationToken token = default)
        {
            var entities = OnMapping(request);

            var validate = await ValidateSaveDelete(request, token);
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

        protected virtual Task BeforeSaveDelete(TRequest request, List<T> entities, CancellationToken token = default) => Task.CompletedTask;
        protected virtual async Task<Result> ValidateSaveDelete(TRequest request, CancellationToken token = default)
        {
            var entities = OnMapping(request);

            var entityIds = entities.Select(x => x.Id).ToList();
            var anyFalseId = GetAll().Where(x => entityIds.Contains(x.Id)).Select(x => x.Id).ToList().Any(x => !entityIds.Contains(x));

            var result = Result.OK();

            if (anyFalseId)
                return Result.Failed(_loc["Entity.IdNotFound", EntityName, string.Join(", ", entityIds)]);

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
