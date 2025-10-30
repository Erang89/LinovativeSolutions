using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.OrderTypes;
using LinoVative.Shared.Dto.OrderTypes;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.OrderTypes
{
    public class GetAllOrderTypeIQueryableCommand : IRequest<IQueryable<OrderTypeViewDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllOrderTypeQueryableHandlerService : QueryServiceBase<OrderType, GetAllOrderTypeIQueryableCommand>, IRequestHandler<GetAllOrderTypeIQueryableCommand, IQueryable<OrderTypeViewDto>>
    {
        public GetAllOrderTypeQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<OrderType> OnGetAllFilter(IQueryable<OrderType> query, GetAllOrderTypeIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => x.Name!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<OrderTypeViewDto>> Handle(GetAllOrderTypeIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<OrderTypeViewDto>(_mapper.Config));

    }
}
