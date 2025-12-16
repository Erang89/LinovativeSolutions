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
    public class GetAllBulkUploadItemDetailIQueryableCommand : IRequest<IQueryable<BulkUploadItemDetailDto>>
    {
        public Guid? Id { get; set; }
        public CrudOperations? Operation { get; set; }
        public string? SearchKeyword { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllItemQueryableDetailHandlerService : QueryServiceBase<ItemBulkUploadDetail, GetAllBulkUploadItemDetailIQueryableCommand>, IRequestHandler<GetAllBulkUploadItemDetailIQueryableCommand, IQueryable<BulkUploadItemDetailDto>>
    {

        public GetAllItemQueryableDetailHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<ItemBulkUploadDetail> OnGetAllFilter(IQueryable<ItemBulkUploadDetail> query, GetAllBulkUploadItemDetailIQueryableCommand req)
        {
            var q = base.OnGetAllFilter(query, req)
                .Where(x => x.ItemBulkUpload!.UserId == _actor.UserId && x.ItemBulkUpload.CompanyId == _actor.CompanyId);

            if(req.Id is not null)
                q = q.Where(x => x.Id == req.Id);

            if (req.Operation is not null)
                q = q.Where(x => x.ItemBulkUpload!.Operation == req.Operation);

            if (!string.IsNullOrEmpty(req.SearchKeyword))
                q = q.Where(x => string.Concat(x.Column1, x.Column2).Contains(req.SearchKeyword??""));

            return q;
        }

        public Task<IQueryable<BulkUploadItemDetailDto>> Handle(GetAllBulkUploadItemDetailIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<BulkUploadItemDetailDto>(_mapper.Config).ApplyFilters(request.Filter));
    }
}
