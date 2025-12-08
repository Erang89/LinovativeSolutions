using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Delete
{
    public enum RemoveBulkUploadItemType
    {
        ItemCreate, 
        ItemUpdate, 
        ItemDelete, 
        ItemMapping, 
        GroupCreate,
        GroupUpdate,
        GroupDelete,
        GroupMapping,
        CategoryCreate,
        CategoryUpdate,
        CategoryDelete,
        CategoryMapping,
        UnitCreate,
        UnitUpdate,
        UnitDelete,
        UnitMapping,
    }
    public class RemoveBulkUploadItemCommand : IRequest<Result>
    {
        public RemoveBulkUploadItemType UploadType { get; set; }
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
            var deleteMainQuery = new Dictionary<RemoveBulkUploadItemType, Func<Task>>() 
            {
                {RemoveBulkUploadItemType.ItemCreate, () => _dbContext.ItemBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Create).ExecuteDeleteAsync()},
                {RemoveBulkUploadItemType.ItemUpdate,  () => _dbContext.ItemBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.ItemDelete,  () => _dbContext.ItemBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.ItemMapping,  () => _dbContext.ItemBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.GroupCreate, () =>  _dbContext.ItemGroupBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.GroupUpdate, () =>  _dbContext.ItemGroupBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.GroupDelete, () =>  _dbContext.ItemGroupBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.GroupMapping, () =>  _dbContext.ItemGroupBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.CategoryCreate, () => _dbContext.ItemCategoryBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.CategoryUpdate, () => _dbContext.ItemCategoryBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.CategoryDelete,  () =>_dbContext.ItemCategoryBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.CategoryMapping,  () =>_dbContext.ItemCategoryBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.UnitCreate,  () => _dbContext.ItemUnitBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.UnitUpdate, () => _dbContext.ItemUnitBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Update).ExecuteDeleteAsync()},
                {RemoveBulkUploadItemType.UnitDelete, () => _dbContext.ItemUnitBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.UnitMapping, () => _dbContext.ItemUnitBulkUploads.Where(x => x.UserId == _actor.UserId && x.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
            };

            var deleteDetailQuery = new Dictionary<RemoveBulkUploadItemType, Func<Task>>()
            {
                {RemoveBulkUploadItemType.ItemCreate, () => _dbContext.ItemBulkUploadDetails.Where(x => x.ItemBulkUpload!.UserId == _actor.UserId && x.ItemBulkUpload!.Operation == CrudOperations.Create).ExecuteDeleteAsync()},
                {RemoveBulkUploadItemType.ItemUpdate, () => _dbContext.ItemBulkUploadDetails.Where(x => x.ItemBulkUpload!.UserId == _actor.UserId && x.ItemBulkUpload!.Operation == CrudOperations.Update).ExecuteDeleteAsync()},
                {RemoveBulkUploadItemType.ItemDelete, () => _dbContext.ItemBulkUploadDetails.Where(x => x.ItemBulkUpload!.UserId == _actor.UserId && x.ItemBulkUpload!.Operation == CrudOperations.Delete).ExecuteDeleteAsync()},
                {RemoveBulkUploadItemType.ItemMapping, () => _dbContext.ItemBulkUploadDetails.Where(x => x.ItemBulkUpload!.UserId == _actor.UserId && x.ItemBulkUpload!.Operation == CrudOperations.Mapping).ExecuteDeleteAsync()},
                {RemoveBulkUploadItemType.GroupCreate,() =>  _dbContext.ItemGroupBulkUploadDetails.Where(x => x.ItemGroupBulkUpload!.UserId == _actor.UserId && x.ItemGroupBulkUpload!.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.GroupUpdate, () =>  _dbContext.ItemGroupBulkUploadDetails.Where(x => x.ItemGroupBulkUpload!.UserId == _actor.UserId && x.ItemGroupBulkUpload!.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.GroupDelete,  () => _dbContext.ItemGroupBulkUploadDetails.Where(x => x.ItemGroupBulkUpload!.UserId == _actor.UserId && x.ItemGroupBulkUpload!.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.GroupMapping,  () => _dbContext.ItemGroupBulkUploadDetails.Where(x => x.ItemGroupBulkUpload!.UserId == _actor.UserId && x.ItemGroupBulkUpload!.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.CategoryCreate, () =>  _dbContext.ItemCategoryBulkUploadDetails.Where(x => x.ItemCategoryBulkUpload!.UserId == _actor.UserId && x.ItemCategoryBulkUpload!.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.CategoryUpdate, () =>  _dbContext.ItemCategoryBulkUploadDetails.Where(x => x.ItemCategoryBulkUpload!.UserId == _actor.UserId && x.ItemCategoryBulkUpload!.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.CategoryDelete, () =>  _dbContext.ItemCategoryBulkUploadDetails.Where(x => x.ItemCategoryBulkUpload!.UserId == _actor.UserId && x.ItemCategoryBulkUpload!.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.CategoryMapping, () =>  _dbContext.ItemCategoryBulkUploadDetails.Where(x => x.ItemCategoryBulkUpload!.UserId == _actor.UserId && x.ItemCategoryBulkUpload!.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.UnitCreate, () =>  _dbContext.ItemUnitBulkUploadDetails.Where(x => x.ItemUnitBulkUpload!.UserId == _actor.UserId && x.ItemUnitBulkUpload.Operation == CrudOperations.Create).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.UnitUpdate, () =>  _dbContext.ItemUnitBulkUploadDetails.Where(x => x.ItemUnitBulkUpload!.UserId == _actor.UserId && x.ItemUnitBulkUpload.Operation == CrudOperations.Update).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.UnitDelete, () =>  _dbContext.ItemUnitBulkUploadDetails.Where(x => x.ItemUnitBulkUpload!.UserId == _actor.UserId && x.ItemUnitBulkUpload.Operation == CrudOperations.Delete).ExecuteDeleteAsync() },
                {RemoveBulkUploadItemType.UnitMapping, () =>  _dbContext.ItemUnitBulkUploadDetails.Where(x => x.ItemUnitBulkUpload!.UserId == _actor.UserId && x.ItemUnitBulkUpload.Operation == CrudOperations.Mapping).ExecuteDeleteAsync() },
            };

            var deleteDetail =  deleteDetailQuery[request.UploadType];
            var deleteMain = deleteMainQuery[request.UploadType];
            await deleteDetail();
            await deleteMain();
            return Result.OK();
        }
    }
}
