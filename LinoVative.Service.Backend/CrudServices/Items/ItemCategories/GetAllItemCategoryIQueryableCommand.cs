using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.ItemDtos;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemCategories
{
    public class GetAllItemCategoryIQueryableCommand : IRequest<IQueryable<ItemCategoryViewDto>>
    {
        public string? SearchKeyword { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllItemCategoryQueryableHandlerService : QueryServiceBase<ItemCategory, GetAllItemCategoryIQueryableCommand>, IRequestHandler<GetAllItemCategoryIQueryableCommand, IQueryable<ItemCategoryViewDto>>
    {
        public GetAllItemCategoryQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<ItemCategory> OnGetAllFilter(IQueryable<ItemCategory> query, GetAllItemCategoryIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Name, x.Description).Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<ItemCategoryViewDto>> Handle(GetAllItemCategoryIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<ItemCategoryViewDto>(_mapper.Config).ApplyFilters(request.Filter));
    }
}
