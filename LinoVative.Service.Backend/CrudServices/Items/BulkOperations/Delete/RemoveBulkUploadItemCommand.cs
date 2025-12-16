using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Enums;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Delete
{
    
    public class RemoveBulkUploadItemCommand : IRequest<Result>
    {
        public BulkOperationTypes UploadType { get; set; }
    }


    public class RemoveBulkUploadItemHandler : IRequestHandler<RemoveBulkUploadItemCommand, Result>
    {

        private readonly IAppDbContext _dbContext;
        private readonly IActor _actor;

        public RemoveBulkUploadItemHandler(IAppDbContext dbContext, IActor actor)
        {
            _dbContext = dbContext;
            _actor = actor;
        }

        public async Task<Result> Handle(RemoveBulkUploadItemCommand request, CancellationToken ct)
        {
            var deleteMainQuery = new Dictionary<BulkOperationTypes, Func<Task>>() 
            {
                {BulkOperationTypes.ItemCreate, () => _dbContext.ItemBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Create).ExecuteDeleteAsync()},
                {BulkOperationTypes.ItemUpdate,  () => _dbContext.ItemBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {BulkOperationTypes.ItemDelete,  () => _dbContext.ItemBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {BulkOperationTypes.ItemMapping,  () => _dbContext.ItemBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
                {BulkOperationTypes.GroupCreate, () =>  _dbContext.ItemGroupBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {BulkOperationTypes.GroupUpdate, () =>  _dbContext.ItemGroupBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {BulkOperationTypes.GroupDelete, () =>  _dbContext.ItemGroupBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {BulkOperationTypes.GroupMapping, () =>  _dbContext.ItemGroupBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
                {BulkOperationTypes.CategoryCreate, () => _dbContext.ItemCategoryBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {BulkOperationTypes.CategoryUpdate, () => _dbContext.ItemCategoryBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {BulkOperationTypes.CategoryDelete,  () =>_dbContext.ItemCategoryBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {BulkOperationTypes.CategoryMapping,  () =>_dbContext.ItemCategoryBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
                {BulkOperationTypes.UnitCreate,  () => _dbContext.ItemUnitBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {BulkOperationTypes.UnitUpdate, () => _dbContext.ItemUnitBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Update).ExecuteDeleteAsync()},
                {BulkOperationTypes.UnitDelete, () => _dbContext.ItemUnitBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {BulkOperationTypes.UnitMapping, () => _dbContext.ItemUnitBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
            };

            var deleteDetailQuery = new Dictionary<BulkOperationTypes, Func<Task>>()
            {
                {BulkOperationTypes.ItemCreate, () => _dbContext.ItemBulkUploadDetails.Where(x => x.ItemBulkUpload!.UserId == _actor.UserId && x.ItemBulkUpload!.Operation == CrudOperations.Create).ExecuteDeleteAsync()},
                {BulkOperationTypes.ItemUpdate, () => _dbContext.ItemBulkUploadDetails.Where(x => x.ItemBulkUpload!.UserId == _actor.UserId && x.ItemBulkUpload!.Operation == CrudOperations.Update).ExecuteDeleteAsync()},
                {BulkOperationTypes.ItemDelete, () => _dbContext.ItemBulkUploadDetails.Where(x => x.ItemBulkUpload!.UserId == _actor.UserId && x.ItemBulkUpload!.Operation == CrudOperations.Delete).ExecuteDeleteAsync()},
                {BulkOperationTypes.ItemMapping, () => _dbContext.ItemBulkUploadDetails.Where(x => x.ItemBulkUpload!.UserId == _actor.UserId && x.ItemBulkUpload!.Operation == CrudOperations.Mapping).ExecuteDeleteAsync()},
                {BulkOperationTypes.GroupCreate,() =>  _dbContext.ItemGroupBulkUploadDetails.Where(x => x.ItemGroupBulkUpload!.UserId == _actor.UserId && x.ItemGroupBulkUpload!.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {BulkOperationTypes.GroupUpdate, () =>  _dbContext.ItemGroupBulkUploadDetails.Where(x => x.ItemGroupBulkUpload!.UserId == _actor.UserId && x.ItemGroupBulkUpload!.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {BulkOperationTypes.GroupDelete,  () => _dbContext.ItemGroupBulkUploadDetails.Where(x => x.ItemGroupBulkUpload!.UserId == _actor.UserId && x.ItemGroupBulkUpload!.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {BulkOperationTypes.GroupMapping,  () => _dbContext.ItemGroupBulkUploadDetails.Where(x => x.ItemGroupBulkUpload!.UserId == _actor.UserId && x.ItemGroupBulkUpload!.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
                {BulkOperationTypes.CategoryCreate, () =>  _dbContext.ItemCategoryBulkUploadDetails.Where(x => x.ItemCategoryBulkUpload!.UserId == _actor.UserId && x.ItemCategoryBulkUpload!.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {BulkOperationTypes.CategoryUpdate, () =>  _dbContext.ItemCategoryBulkUploadDetails.Where(x => x.ItemCategoryBulkUpload!.UserId == _actor.UserId && x.ItemCategoryBulkUpload!.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {BulkOperationTypes.CategoryDelete, () =>  _dbContext.ItemCategoryBulkUploadDetails.Where(x => x.ItemCategoryBulkUpload!.UserId == _actor.UserId && x.ItemCategoryBulkUpload!.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {BulkOperationTypes.CategoryMapping, () =>  _dbContext.ItemCategoryBulkUploadDetails.Where(x => x.ItemCategoryBulkUpload!.UserId == _actor.UserId && x.ItemCategoryBulkUpload!.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
                {BulkOperationTypes.UnitCreate, () =>  _dbContext.ItemUnitBulkUploadDetails.Where(x => x.ItemUnitBulkUpload!.UserId == _actor.UserId && x.ItemUnitBulkUpload.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {BulkOperationTypes.UnitUpdate, () =>  _dbContext.ItemUnitBulkUploadDetails.Where(x => x.ItemUnitBulkUpload!.UserId == _actor.UserId && x.ItemUnitBulkUpload.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {BulkOperationTypes.UnitDelete, () =>  _dbContext.ItemUnitBulkUploadDetails.Where(x => x.ItemUnitBulkUpload!.UserId == _actor.UserId && x.ItemUnitBulkUpload.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {BulkOperationTypes.UnitMapping, () =>  _dbContext.ItemUnitBulkUploadDetails.Where(x => x.ItemUnitBulkUpload!.UserId == _actor.UserId && x.ItemUnitBulkUpload.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
            };

            var deleteDetail =  deleteDetailQuery[request.UploadType];
            var deleteMain = deleteMainQuery[request.UploadType];
            await deleteDetail();
            await deleteMain();
            return Result.OK();
        }
    }
}
