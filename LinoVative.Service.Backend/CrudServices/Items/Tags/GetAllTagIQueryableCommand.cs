using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.ItemDtos;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Items.Tags
{
    public class GetAllTagIQueryableCommand : IRequest<IQueryable<TagDto>>
    {
        public string? SearchKeyword { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllTagQueryableHandlerService : QueryServiceBase<Tag, GetAllTagIQueryableCommand>, IRequestHandler<GetAllTagIQueryableCommand, IQueryable<TagDto>>
    {
        public GetAllTagQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Tag> OnGetAllFilter(IQueryable<Tag> query, GetAllTagIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => x.Name!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<TagDto>> Handle(GetAllTagIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<TagDto>(_mapper.Config).ApplyFilters(request.Filter));
    }
}
