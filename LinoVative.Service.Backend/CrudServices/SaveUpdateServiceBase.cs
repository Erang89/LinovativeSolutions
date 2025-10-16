using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Extensions;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices
{
    public abstract class SaveUpdateServiceBase<T, TRequest> : QueryServiceBase<T> where T : class, IEntityId where TRequest : class, IEntityId
    {
        private readonly IStringLocalizer _stringLocalizer;

        protected SaveUpdateServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer stringLocalizer) : base(dbContext, actor, mapper, appCache)
        { 
            _stringLocalizer = stringLocalizer;
        }


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


        // ============================== Save New ==============================
        protected virtual async Task<Result> SaveNew(TRequest request, CancellationToken token)
        {
            var validate = await ValidateSaveNew(request, token);
            if (!validate)
                return validate;
            
            T entity = await OnMapping(request!);

            if (typeof(IsEntityManageByClinet).IsAssignableFrom(typeof(T)))
            {
               ((IsEntityManageByClinet)entity).ClientId = _actor.CompanyId;
            }

            await BeforeSaveNew(request, entity, token);

            _dbSet.Add(entity);

            var result = await _dbContext.SaveAsync(_actor, token);
            if(result) return Result.OK(entity);

            return result;

        }
        protected virtual Task BeforeSaveNew(TRequest request, T entity, CancellationToken token) => Task.CompletedTask;
        protected virtual async Task<Result> ValidateSaveNew<TRequest>(TRequest request, CancellationToken token)
        {
            
            await Task.CompletedTask;
            return Result.OK();
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
            
            if (typeof(IsEntityManageByClinet).IsAssignableFrom(typeof(T)))
            {
                ((IsEntityManageByClinet)entity!).ClientId = _actor.CompanyId;
            }

            return await _dbContext.SaveAsync(_actor, token);
        }
        protected virtual Task BeforeSaveUpdate(TRequest request, T entity, CancellationToken token) => Task.CompletedTask;
        protected virtual async Task<Result> ValidateSaveUpdate(TRequest request, CancellationToken token)
        {
            var validate = request.ValidateRequiredPropery(_stringLocalizer);
            if (!validate) return validate;
            
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




    }
}
