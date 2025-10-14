using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Commons;
using LinoVative.Service.Core.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices
{
    public abstract class SaveDeleteServiceBase<T> : QueryServiceBase<T> where T : class, IEntityId
    {
        protected SaveDeleteServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IActionContextAccessor actionContext) : base(dbContext, actor, mapper, appCache)
        { 
        }


        // ==============================  Delete ==============================
        protected virtual async Task<Result> SaveDelete<TRequest>(TRequest request, Guid id, CancellationToken token = default)
        {
            var entity = await GetById(id);
            if (entity is null)
                return Result.Failed($"Entity with ID: {id} not found");

            return await SaveDelete(request, new List<T>() { entity }, token);
        }

        protected virtual Task<Result> SaveDelete<TRequest>(TRequest request,  List<Guid> ids, CancellationToken token = default)
        {
            var entities = GetAll().Where(x => ids.Contains(x.Id)).ToList();
            return SaveDelete(request, entities, token);
        }

        protected virtual async Task<Result> SaveDelete<TRequest>(TRequest request,  List<T> entities, CancellationToken token = default)
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

        protected virtual Task<Result> SaveDelete<TRequest>(TRequest request,  T entity, CancellationToken token = default)
        {
            return SaveDelete(request, new List<T>() { entity}, token);
        }


        protected virtual Task BeforeSaveDelete<TRequest>(TRequest request, List<T> entities, CancellationToken token = default) => Task.CompletedTask;
        protected virtual async Task<Result> ValidateSaveDelete<TRequest>(TRequest request, List<T> entities, CancellationToken token = default)
        {
            var entityIds = entities.Select(x => x.Id).ToList();
            var anyFalseId = GetAll().Where(x => entityIds.Contains(x.Id)).Select(x => x.Id).ToList().Any(x => !entityIds.Contains(x));
            
            if (anyFalseId) return Result.Failed("Some Entity not exisit in the system");


            await Task.CompletedTask;
            return Result.OK();
        }

    }
}
