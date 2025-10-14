using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Commons;
using LinoVative.Service.Core.Interfaces;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices
{
    public abstract class SaveUpdateServiceBase<T> : QueryServiceBase<T> where T : class, IEntityId
    {
        protected SaveUpdateServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        { 
        }


        // ============================== Maping Entity ==============================
        protected virtual async Task<T> OnMapping<TRequest>(TRequest request)
        {
            await Task.CompletedTask;
            return _mapper.Map<T>(request!);
        }

        protected virtual async Task<T> OnMapping<TRequest>(TRequest request, T entity)
        {
            await Task.CompletedTask;
            return _mapper.Map(request!, entity);
        }


        // ============================== Save New ==============================
        protected virtual async Task<Result> SaveNew<TRequest>(TRequest request, CancellationToken token)
        {
            var validate = await ValidateSaveNew(request, token);
            if (!validate)
                return validate;
            
            T entity = await OnMapping(request!);

            if (typeof(IsEntityManageByClinet).IsAssignableFrom(typeof(T)))
            {
               ((IsEntityManageByClinet)entity).ClientId = _actor.ClientId;
            }

            await BeforeSaveNew(request, entity, token);

            _dbSet.Add(entity);

            var result = await _dbContext.SaveAsync(_actor, token);
            if(result) return Result.OK(entity);

            return result;

        }
        protected virtual Task BeforeSaveNew<TRequest>(TRequest request, T entity, CancellationToken token) => Task.CompletedTask;
        protected virtual async Task<Result> ValidateSaveNew<TRequest>(TRequest request, CancellationToken token)
        {
            
            await Task.CompletedTask;
            return Result.OK();
        }



        // ============================== Update ==============================
        protected virtual async Task<Result> SaveUpdate<TRequest>(TRequest request, CancellationToken token) where TRequest : class, IEntityId
        {
            var validate = await ValidateSaveUpdate(request, token);
            if (!validate)
                return validate;

            T entity = (await GetById(request.Id))!;

            entity = await OnMapping(request, entity);

            await BeforeSaveUpdate(request, entity, token);
            
            if (typeof(IsEntityManageByClinet).IsAssignableFrom(typeof(T)))
            {
                ((IsEntityManageByClinet)entity!).ClientId = _actor.ClientId;
            }

            return await _dbContext.SaveAsync(_actor, token);
        }
        protected virtual Task BeforeSaveUpdate<TRequest>(TRequest request, T entity, CancellationToken token) => Task.CompletedTask;
        protected virtual async Task<Result> ValidateSaveUpdate<TRequest>(TRequest request, CancellationToken token) where TRequest : class, IEntityId
        {
            
            var entity = await GetById(request.Id);
            if (entity is null)
                return Result.Failed($"Entity with Id: {request.Id} not found");

            if (!await _actor.CanUpdateEntity(entity, _dbContext))
            {
                return Result.Failed("Unauthorized Operation", "You don't have permission to run the operation", new(), System.Net.HttpStatusCode.Unauthorized);
            }

            await Task.CompletedTask;
            return Result.OK();
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

            foreach(var entity in entities)
            {
                if(!await _actor.CanDeleteEntity(entity, _dbContext))
                {
                    return Result.Failed("Unauthorized Operation", "You don't have permission to run the operation", new(), System.Net.HttpStatusCode.Unauthorized);
                }
            }

            await Task.CompletedTask;
            return Result.OK();
        }

    }
}
