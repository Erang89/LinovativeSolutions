using Azure;
using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Formats.Tar;
using System.Linq.Expressions;

namespace LinoVative.Service.Backend.CrudServices
{
    public abstract class SaveNewServiceBase<T, TRequest> : QueryServiceBase<T>, IRequestHandler<TRequest, Result>
        where T : class, IEntityId where TRequest : class, IRequest<Result>
    {
        protected IStringLocalizer _localizer;
        protected SaveNewServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) 
            : base(dbContext, actor, mapper, appCache)
        { 
            _localizer = localizer;
        }


        // ============================== Maping Entity ==============================
        protected virtual async Task<T> OnMapping(TRequest request, CancellationToken ct)
        {
            await Task.CompletedTask;
            var entity = _mapper.Map<T>(request!);

            if (typeof(IsEntityManageByCompany).IsAssignableFrom(typeof(T)))
            {
                ((IsEntityManageByCompany)entity).CompanyId = _actor.CompanyId;
            }

            return entity;
        }

        public virtual async Task<Result> Handle(TRequest request, CancellationToken ct)
        {
            var validate = await Validate(request, ct);
            if (!validate)
                return validate;
            
            var creatingResult = await OnCreatingEntity(request, ct);

            foreach (var entity in creatingResult)
                await BeforeSave(request, entity, ct);

            _dbSet.AddRange(creatingResult);

            if (typeof(IAuditableEntity).IsAssignableFrom(typeof(T)))
            {
                foreach (IAuditableEntity entity in creatingResult)
                {
                    if(_dbContext.GetEntityState(entity) == EntityState.Added)
                    {
                        entity.CreateBy(_actor);
                    }
                }
            }


            if (typeof(IsEntityManageByCompany).IsAssignableFrom(typeof(T)))
            {
                foreach (IsEntityManageByCompany entity in creatingResult)
                {
                    if (_dbContext.GetEntityState(entity) == EntityState.Added && entity.CompanyId is null)
                    {
                        entity.CompanyId = _actor.CompanyId;
                    }
                }
            }

                var result = await _dbContext.SaveAsync(_actor, ct);
            if(result) return Result.OK(creatingResult.Select(x => x.Id).ToList());

            return Result.OK(creatingResult.Select(x => x.Id).ToList());
        }


        public virtual Task BeforeSave(TRequest request, T entity, CancellationToken token) => Task.CompletedTask;


        protected virtual async Task<List<T>> OnCreatingEntity(TRequest request, CancellationToken token = default)
        {
            T entity = await OnMapping(request!, token);
            return new() { entity };
        }


        protected virtual async Task<Result> Validate(TRequest request, CancellationToken token) => Result.OK();

        protected string Prop(Expression<Func<TRequest, object>> expresion) => DtoExtensions.GetPropertyName(expresion);

        protected void AddError(Result result, string propertyKey, string errorMessage) =>
            result.AddInvalidProperty(propertyKey, errorMessage);

        protected void AddError(Result result, Expression<Func<TRequest, object>> expresion, string message) =>
            result.AddInvalidProperty(Prop(expresion), message);

        protected void AddError(Result result, Expression<Func<TRequest, object>> expresion, object obj) =>
            result.AddInvalidProperty(Prop(expresion), obj);

    }
}
