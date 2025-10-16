using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Bases;
using Mapster;
using MapsterMapper;
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

        protected QueryServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
            _actor = actor;
            _appCache = appCache;
            _mapper = mapper;
        }


        // ============================== Queries ==============================
        protected IQueryable<T> GetAll(bool includeDeleted = false)
        {
            var query = _dbSet.Where(_ => true);

            if (typeof(IDeleteableEntity).IsAssignableFrom(typeof(T)) && !includeDeleted)
            {
                query = query.Where(e => !((IDeleteableEntity)e).IsDeleted);
            }

            if (typeof(IsEntityManageByClinet).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => ((IsEntityManageByClinet)e).ClientId == _actor.CompanyId);
            }

            return query;
        }
        protected async Task<T?> GetById(Guid id) => await _appCache.Get<T>($"entity{id}", async () => await _dbSet.FirstOrDefaultAsync(x => x.Id == id));


    }

    public abstract class PaginationQueryServiceBase<T, TReq> : QueryServiceBase<T> where T : class, IEntityId
    {
        protected PaginationQueryServiceBase(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) :
            base(dbContext, actor, mapper, appCache)
        {
        }

        // Pagination Query
        protected virtual IQueryable<T> OnPaginationQueryFilter(IQueryable<T> query, TReq request) => query;
        protected virtual IQueryable<TResponse> OnPaginationMapping<TResponse>(IQueryable<T> query) => query.ProjectToType<TResponse>(_mapper.Config);
        protected virtual IQueryable<TResponse> OnApplyPagination<TResponse>(IQueryable<TResponse> responseQuery, IPagination? pagination)
        {
            if (pagination is null)
                return responseQuery;

            if (pagination.Page is null || pagination.Page <= 0) pagination.Page = 1;
            if (pagination.PageSize is null || pagination.PageSize > 100) pagination.PageSize = 100;

            return responseQuery
                .Skip((pagination.Page!.Value - 1) * pagination.PageSize!.Value)
                .Take(pagination.PageSize!.Value);
        }


        protected virtual IQueryable<TResponse> PaginationQuery<TResponse>(TReq request)
        {
            var query = GetAll();
            query = OnPaginationQueryFilter(query, request);
            var responseQuery = OnPaginationMapping<TResponse>(query);
            
            if(request is IPagination pagination)
                return OnApplyPagination(responseQuery, pagination);

            return responseQuery;
        }


        protected virtual async Task<Result> GetPaginationResult<TResult>(TReq request)
        {
            var filteredQuery = OnPaginationQueryFilter(GetAll(), request);
            var recordCount = await filteredQuery.CountAsync();

            var paginationQuery = OnPaginationMapping<TResult>(filteredQuery);
            if (request is IPagination pagination)
                paginationQuery =  OnApplyPagination(paginationQuery, pagination);

            var data = await paginationQuery.ToListAsync();
            return Result.ListOfData(data, recordCount);
        }
    }
}
