using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.ItemDtos;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.Items
{
    public class GetAllSKUItemIQueryableCommand : IRequest<IQueryable<SKUItemViewDto>>
    {
        public string? SearchKeyword { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllSKUItemQueryableHandlerService : QueryServiceBase<SKUItem, GetAllSKUItemIQueryableCommand>, IRequestHandler<GetAllSKUItemIQueryableCommand, IQueryable<SKUItemViewDto>>
    {
        public GetAllSKUItemQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<SKUItem> OnGetAllFilter(IQueryable<SKUItem> query, GetAllSKUItemIQueryableCommand req)
        {
            if (string.IsNullOrWhiteSpace(req.SearchKeyword)) return query;

            var q = base.OnGetAllFilter(query, req).Where(x => string.Concat(x.SKU, x.VarianName).Contains(req.SearchKeyword??""));
            return q;
        }

        public async Task<IQueryable<SKUItemViewDto>> Handle(GetAllSKUItemIQueryableCommand request, CancellationToken ct)
        {
            var q = base.GetAll(request).ProjectToType<SKUItemViewDto>(_mapper.Config).ApplyFilters(request.Filter);
            
            return q;
        }
            
    }
}
