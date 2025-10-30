using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Warehoses;
using LinoVative.Shared.Dto.MasterData.Warehouses;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Warehouses
{
    public class GetAllWarehouseIQueryableCommand : IRequest<IQueryable<WarehouseDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllWarehouseQueryableHandlerService : QueryServiceBase<Warehouse, GetAllWarehouseIQueryableCommand>, IRequestHandler<GetAllWarehouseIQueryableCommand, IQueryable<WarehouseDto>>
    {
        public GetAllWarehouseQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Warehouse> OnGetAllFilter(IQueryable<Warehouse> query, GetAllWarehouseIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => x.Name!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<WarehouseDto>> Handle(GetAllWarehouseIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<WarehouseDto>(_mapper.Config));

    }
}
