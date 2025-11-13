using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.ItemDtos;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Items.Units
{
    public class GetAllItemUnitIQueryableCommand : IRequest<IQueryable<ItemUnitDto>>
    {
        public string? SearchKeyword { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllItemUnitQueryableHandlerService : QueryServiceBase<ItemUnit, GetAllItemUnitIQueryableCommand>, IRequestHandler<GetAllItemUnitIQueryableCommand, IQueryable<ItemUnitDto>>
    {
        public GetAllItemUnitQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<ItemUnit> OnGetAllFilter(IQueryable<ItemUnit> query, GetAllItemUnitIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Name, x.Description).Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<ItemUnitDto>> Handle(GetAllItemUnitIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<ItemUnitDto>(_mapper.Config).ApplyFilters(request.Filter));
    }
}
