using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices
{
    public abstract class QueryServiceBase<T> where T : class, IEntityId
    {
        protected DbSet<T> _dbSet { get; set; }
        protected readonly IAppDbContext _dbContext;
        protected readonly IActor _actor;
        protected readonly IAppCache _appCache;
        protected readonly IMapper _mapper;
        protected readonly IActionContextAccessor _actionAccessor;

        protected QueryServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IActionContextAccessor actionContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
            _actor = actor;
            _appCache = appCache;
            _mapper = mapper;
            _actionAccessor = actionContext;
        }


        // ============================== Queries ==============================
        protected IQueryable<T> GetAll(bool includeDeleted = false)
        {
            var query = _dbSet.Where(_ => true);

            if (typeof(IDeleteableEntity).IsAssignableFrom(typeof(T)) && !includeDeleted)
            {
                query =  query.Where(e => !((IDeleteableEntity)e).IsDeleted);
            }

            if (typeof(IsEntityManageByClinet).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => ((IsEntityManageByClinet)e).ClientId == _actor.ClientId);
            }

            return query;
        }
        protected async Task<T?> GetById(Guid id) => await _appCache.Get<T>($"entity{id}", async ()=> await _dbSet.FirstOrDefaultAsync(x => x.Id == id));

    }
}
