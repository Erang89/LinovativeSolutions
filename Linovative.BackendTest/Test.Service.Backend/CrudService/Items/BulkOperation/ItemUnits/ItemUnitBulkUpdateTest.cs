using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Group;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Unit;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.ItemUnits
{
    public class ItemUnitBulkUpdateTest : UseDatabaseTestBase
    {
        const string resources = "BulkUploadCommand";

        [Fact]
        public async Task BulkUpdateItemUnit_Submiting_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemUnit1 = new ItemUnit() {CompanyId = _actor.CompanyId, Name = "Item Unit 1" };
            var itemUnit2 = new ItemUnit() {CompanyId = _actor.CompanyId, Name = "Item Unit 2" };
            var itemUnit3 = new ItemUnit() {CompanyId = Guid.NewGuid(), Name = "Update 2" };
            dbContext.ItemUnits.Add(itemUnit1);
            dbContext.ItemUnits.Add(itemUnit2);
            dbContext.ItemUnits.Add(itemUnit3);

            var groupBulkUpload = new ItemUnitBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Update, UserId = _actor.UserId, CompanyId = _actor.CompanyId };
            dbContext.ItemUnitBulkUploads.Add(groupBulkUpload);

            var uploadDetail1 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = groupBulkUpload.Id, Column1 = itemUnit1.Id.ToString(), Column2 = "Update Unit 1" };
            var uploadDetail2 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = groupBulkUpload.Id, Column1 = itemUnit2.Id.ToString(), Column2 = "Update 2" };            
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail2);

            await dbContext.SaveAsync(_actor);

            var bulkUpdateService = new BulkUpdateUnitService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemUnitDto.Id), nameof(ItemUnitBulkUploadDetail.Column1) },
                {nameof(ItemUnitDto.Name), nameof(ItemUnitBulkUploadDetail.Column2) },
            };


            var result1 = await bulkUpdateService.Save([], cts.Token);
            var result2 = await bulkUpdateService.Save(fieldMapping, cts.Token);
            var detail1 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == itemUnit1.Id);
            var detail2 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == itemUnit2.Id);

            Assert.False(result1);
            Assert.True(result2);
            Assert.NotNull(detail1);
            Assert.NotNull(detail2);
            Assert.Equal("Update Unit 1", detail1.Name);
            Assert.Equal("Update 2", detail2.Name);
            Assert.Equal(_actor.UserId, detail2.LastModifiedBy);

            cts.Dispose();
        }



        [Fact]
        public async Task BulkUpdateItemUnit_FailedDueToInvalidID()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemUnit1 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 1" };
            var itemUnit2 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 2" };
            dbContext.ItemUnits.Add(itemUnit1);
            dbContext.ItemUnits.Add(itemUnit2);

            var upload = new ItemUnitBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Update, UserId = _actor.UserId, CompanyId = _actor.CompanyId };
            dbContext.ItemUnitBulkUploads.Add(upload);

            var uploadDetail1 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = upload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Update Unit 1" };
            var uploadDetail2 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = upload.Id, Column1 = itemUnit2.Id.ToString(), Column2 = "Update 2" };
            var uploadDetail3 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = upload.Id, Column1 = "", Column2 = "Update 2" };
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail3);

            await dbContext.SaveAsync(_actor);

            var bulkUpdateService = new BulkUpdateUnitService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemUnitDto.Id), nameof(ItemUnitBulkUploadDetail.Column1) },
                {nameof(ItemUnitDto.Name), nameof(ItemUnitBulkUploadDetail.Column2) },
            };


            var result1 = await bulkUpdateService.Save([], cts.Token);
            var result2 = await bulkUpdateService.Save(fieldMapping, cts.Token);
            var unit1 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == itemUnit1.Id);
            var unit2 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == itemUnit2.Id);
            var error1 = _langService.Format($"{resources}.ValueNotFoundInTheSystem.Message", uploadDetail1.Column1);
            var error2 = _langService.Format($"{resources}.KeyColumnIsEmpty.Message", upload.headerColum1);

            Assert.False(result1);
            Assert.False(result2);
            Assert.Equal("Item Unit 1", unit1!.Name);
            Assert.Equal("Item Unit 2", unit2!.Name);
            Assert.Contains(error1, uploadDetail1.Errors);
            Assert.Contains(error2, uploadDetail3.Errors);

            cts.Dispose();

        }
    }
}
