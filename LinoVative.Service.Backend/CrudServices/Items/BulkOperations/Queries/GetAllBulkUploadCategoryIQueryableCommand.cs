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
    public class GetAllBulkUploadCategoryIQueryableCommand : IRequest<IQueryable<BulkUploadItemCategoryDto>>
    {
        public Guid? Id { get; set; }
        public CrudOperations? Operation { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllItemCategoryQueryableHandlerService : QueryServiceBase<ItemCategoryBulkUpload, GetAllBulkUploadCategoryIQueryableCommand>, IRequestHandler<GetAllBulkUploadCategoryIQueryableCommand, IQueryable<BulkUploadItemCategoryDto>>
    {

        public GetAllItemCategoryQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<ItemCategoryBulkUpload> OnGetAllFilter(IQueryable<ItemCategoryBulkUpload> query, GetAllBulkUploadCategoryIQueryableCommand req)
        {
            var q = base.OnGetAllFilter(query, req).Where(x => x.UserId == _actor.UserId && x.CompanyId == _actor.CompanyId);

            if(req.Id is not null)
                q = q.Where(x => x.Id == req.Id);

            if (req.Operation is not null)
                q = q.Where(x => x.Operation == req.Operation);

            return q;
        }

        public Task<IQueryable<BulkUploadItemCategoryDto>> Handle(GetAllBulkUploadCategoryIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<BulkUploadItemCategoryDto>(_mapper.Config).ApplyFilters(request.Filter));
    }
}
