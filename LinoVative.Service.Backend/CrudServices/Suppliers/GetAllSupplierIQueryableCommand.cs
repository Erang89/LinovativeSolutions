using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Suppliers;
using LinoVative.Shared.Dto.MasterData.Suppliers;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Suppliers
{
    public class GetAllSupplierIQueryableCommand : IRequest<IQueryable<SupplierDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllSupplierQueryableHandlerService : QueryServiceBase<Supplier, GetAllSupplierIQueryableCommand>, IRequestHandler<GetAllSupplierIQueryableCommand, IQueryable<SupplierDto>>
    {
        public GetAllSupplierQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Supplier> OnGetAllFilter(IQueryable<Supplier> query, GetAllSupplierIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Name, x.Code)!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<SupplierDto>> Handle(GetAllSupplierIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<SupplierDto>(_mapper.Config));

    }
}
