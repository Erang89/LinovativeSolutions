using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.ItemDtos;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemGroups
{
    public class GetAllItemGroupIQueryableCommand : IRequest<IQueryable<ItemGroupDto>>
    {
        public string? SearchKeyword { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllItemGroupQueryableHandlerService : QueryServiceBase<ItemGroup, GetAllItemGroupIQueryableCommand>, IRequestHandler<GetAllItemGroupIQueryableCommand, IQueryable<ItemGroupDto>>
    {
        public GetAllItemGroupQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<ItemGroup> OnGetAllFilter(IQueryable<ItemGroup> query, GetAllItemGroupIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Name, x.Description).Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<ItemGroupDto>> Handle(GetAllItemGroupIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<ItemGroupDto>(_mapper.Config).ApplyFilters(request.Filter));
    }
}
