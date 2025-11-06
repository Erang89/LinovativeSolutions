using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using MapsterMapper;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using LinoVative.Shared.Dto;
using LinoVative.Service.Backend.Extensions;

namespace LinoVative.Service.Backend.CrudServices
{
    public abstract class SaveNewServiceBase<T, TRequest> : QueryServiceBase<T> where T : class, IEntityId where TRequest : class
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

        public virtual async Task<Result> Handle(TRequest request, CancellationToken token)
        {
            var validate = await Validate(request, token);
            if (!validate)
                return validate;
            
            var creatingResult = await OnCreatingEntity(request, token);

            _dbSet.AddRange(creatingResult);

            var result = await _dbContext.SaveAsync(_actor, token);
            if(result) return Result.OK(creatingResult.Select(x => x.Id).ToList());

            return Result.OK(creatingResult.Select(x => x.Id).ToList());
        }
        

        protected virtual async Task<List<T>> OnCreatingEntity(TRequest request, CancellationToken token = default)
        {
            T entity = await OnMapping(request!);

            if (typeof(IsEntityManageByCompany).IsAssignableFrom(typeof(T)))
            {
                ((IsEntityManageByCompany)entity).CompanyId = _actor.CompanyId;
            }

            return new() { entity };
        }


        protected virtual async Task<Result> Validate(TRequest request, CancellationToken token) => Result.OK();

        protected string Prop(Expression<Func<TRequest, object>> expresion) => DtoExtensions.GetPropertyName(expresion);

        protected void AddError(Result result, Expression<Func<TRequest, object>> expresion, string message) =>
            result.AddInvalidProperty(Prop(expresion), message);

        protected void AddError(Result result, Expression<Func<TRequest, object>> expresion, object obj) =>
            result.AddInvalidProperty(Prop(expresion), obj);

    }
}
