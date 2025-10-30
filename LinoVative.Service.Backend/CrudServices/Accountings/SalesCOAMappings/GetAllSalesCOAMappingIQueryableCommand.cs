using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto.MasterData.Accountings;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.SalesCOAMappings
{
    public class GetAllSalesCOAMappingIQueryableCommand : IRequest<IQueryable<SalesCOAMappingViewDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllSalesCOAMappingQueryableHandlerService : QueryServiceBase<SalesCOAMapping, GetAllSalesCOAMappingIQueryableCommand>, IRequestHandler<GetAllSalesCOAMappingIQueryableCommand, IQueryable<SalesCOAMappingViewDto>>
    {
        public GetAllSalesCOAMappingQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<SalesCOAMapping> OnGetAllFilter(IQueryable<SalesCOAMapping> query, GetAllSalesCOAMappingIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req);
        }

        public Task<IQueryable<SalesCOAMappingViewDto>> Handle(GetAllSalesCOAMappingIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<SalesCOAMappingViewDto>(_mapper.Config));

    }
}
