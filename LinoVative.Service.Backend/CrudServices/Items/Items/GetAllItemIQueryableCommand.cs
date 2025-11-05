using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Items.Items
{
    public class GetAllItemIQueryableCommand : IRequest<IQueryable<ItemDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllItemQueryableHandlerService : QueryServiceBase<Item, GetAllItemIQueryableCommand>, IRequestHandler<GetAllItemIQueryableCommand, IQueryable<ItemDto>>
    {
        public GetAllItemQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Item> OnGetAllFilter(IQueryable<Item> query, GetAllItemIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Name, x.Code, x.Description).Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<ItemDto>> Handle(GetAllItemIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<ItemDto>(_mapper.Config));
    }
}
