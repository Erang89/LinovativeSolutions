using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using LinoVative.Shared.Dto.Commons;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Queries
{
    public class GetAllBulkUploadGroupDetailIQueryableCommand : IRequest<IQueryable<BulkUploadItemGroupDetailDto>>
    {
        public Guid? Id { get; set; }
        public CrudOperations? Operation { get; set; }
        public string? SearchKeyword { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllItemGroupQueryableDetailHandlerService : QueryServiceBase<ItemGroupBulkUploadDetail, GetAllBulkUploadGroupDetailIQueryableCommand>, IRequestHandler<GetAllBulkUploadGroupDetailIQueryableCommand, IQueryable<BulkUploadItemGroupDetailDto>>
    {

        public GetAllItemGroupQueryableDetailHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<ItemGroupBulkUploadDetail> OnGetAllFilter(IQueryable<ItemGroupBulkUploadDetail> query, GetAllBulkUploadGroupDetailIQueryableCommand req)
        {
            var q = base.OnGetAllFilter(query, req)
                .Where(x => x.ItemGroupBulkUpload!.UserId == _actor.UserId && x.ItemGroupBulkUpload.CompanyId == _actor.CompanyId);

            if(req.Id is not null)
                q = q.Where(x => x.Id == req.Id);

            if (req.Operation is not null)
                q = q.Where(x => x.ItemGroupBulkUpload!.Operation == req.Operation);

            if (!string.IsNullOrEmpty(req.SearchKeyword))
                q = q.Where(x => string.Concat(x.Column1).Contains(req.SearchKeyword??""));

            return q;
        }

        public Task<IQueryable<BulkUploadItemGroupDetailDto>> Handle(GetAllBulkUploadGroupDetailIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<BulkUploadItemGroupDetailDto>(_mapper.Config).ApplyFilters(request.Filter));
    }
}
