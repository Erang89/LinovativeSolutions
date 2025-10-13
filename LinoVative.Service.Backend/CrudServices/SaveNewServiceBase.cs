using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Commons;
using LinoVative.Service.Core.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LinoVative.Service.Backend.CrudServices
{
    public abstract class SaveNewServiceBase<T, TRequest> : QueryServiceBase<T> where T : class, IEntityId
    {
        protected SaveNewServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IActionContextAccessor actionContext) : base(dbContext, actor, mapper, appCache, actionContext)
        { 
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
            var actionContext = _actionAccessor.ActionContext;
            if (actionContext != null && !actionContext.ModelState.IsValid)
            {
                var errorMessage = actionContext?.ModelState.GetErrorMessages();
                return Result.Failed("Some input fields are errors", "Input Errors", errorMessage, System.Net.HttpStatusCode.BadRequest);
            }

            await Task.CompletedTask;
            return Result.OK();
        }


    }
}
