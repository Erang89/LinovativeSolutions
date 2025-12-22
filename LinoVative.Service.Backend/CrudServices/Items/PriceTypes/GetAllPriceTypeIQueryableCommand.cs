using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.ItemDtos;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Items.PriceTypes
{
    public class GetAllPriceTypeIQueryableCommand : IRequest<IQueryable<PriceTypeDto>>
    {
        public string? SearchKeyword { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllPriceTypeQueryableHandlerService : QueryServiceBase<PriceType, GetAllPriceTypeIQueryableCommand>, IRequestHandler<GetAllPriceTypeIQueryableCommand, IQueryable<PriceTypeDto>>
    {
        public GetAllPriceTypeQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<PriceType> OnGetAllFilter(IQueryable<PriceType> query, GetAllPriceTypeIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Name, x.Description).Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<PriceTypeDto>> Handle(GetAllPriceTypeIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<PriceTypeDto>(_mapper.Config).ApplyFilters(request.Filter));
    }
}
