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
    public class GetAllBulkUploadUnitDetailIQueryableCommand : IRequest<IQueryable<BulkUploadItemUnitDetailDto>>
    {
        public Guid? Id { get; set; }
        public CrudOperations? Operation { get; set; }
        public string? SearchKeyword { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllItemUnitQueryableDetailHandlerService : QueryServiceBase<ItemUnitBulkUploadDetail, GetAllBulkUploadUnitDetailIQueryableCommand>, IRequestHandler<GetAllBulkUploadUnitDetailIQueryableCommand, IQueryable<BulkUploadItemUnitDetailDto>>
    {

        public GetAllItemUnitQueryableDetailHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<ItemUnitBulkUploadDetail> OnGetAllFilter(IQueryable<ItemUnitBulkUploadDetail> query, GetAllBulkUploadUnitDetailIQueryableCommand req)
        {
            var q = base.OnGetAllFilter(query, req)
                .Where(x => x.ItemUnitBulkUpload!.UserId == _actor.UserId && x.ItemUnitBulkUpload.CompanyId == _actor.CompanyId);

            if(req.Id is not null)
                q = q.Where(x => x.Id == req.Id);

            if (req.Operation is not null)
                q = q.Where(x => x.ItemUnitBulkUpload!.Operation == req.Operation);

            if (!string.IsNullOrEmpty(req.SearchKeyword))
                q = q.Where(x => string.Concat(x.Column1).Contains(req.SearchKeyword??""));

            return q;
        }

        public Task<IQueryable<BulkUploadItemUnitDetailDto>> Handle(GetAllBulkUploadUnitDetailIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<BulkUploadItemUnitDetailDto>(_mapper.Config).ApplyFilters(request.Filter));
    }
}
