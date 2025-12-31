using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Group;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.ItemGroups
{
    public class ItemGroupBulkUpdateTest : UseDatabaseTestBase
    {
        const string resources = "BulkUploadCommand";

        [Fact]
        public async Task BulkUpdateItemGroup_Submiting_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemGroup1 = new ItemGroup() {CompanyId = _actor.CompanyId, Name = "Item Group 1" };
            var itemGroup2 = new ItemGroup() {CompanyId = _actor.CompanyId, Name = "Item Group 2" };
            var itemGroup3 = new ItemGroup() {CompanyId = Guid.NewGuid(), Name = "Update 2" };
            dbContext.ItemGroups.Add(itemGroup1);
            dbContext.ItemGroups.Add(itemGroup2);
            dbContext.ItemGroups.Add(itemGroup3);

            var groupBulkUpload = new ItemGroupBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Update, UserId = _actor.UserId, CompanyId = _actor.CompanyId };
            dbContext.ItemGroupBulkUploads.Add(groupBulkUpload);

            var uploadDetail1 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup1.Id.ToString(), Column2 = "Update Group 1" };
            var uploadDetail2 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup2.Id.ToString(), Column2 = "Update 2" };            
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail2);

            await dbContext.SaveAsync(_actor);

            var bulkUpdateService = new BulkUpdateGroupService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
            };


            var result1 = await bulkUpdateService.Save([], cts.Token);
            var result2 = await bulkUpdateService.Save(fieldMapping, cts.Token);
            var detail1 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == itemGroup1.Id);
            var detail2 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == itemGroup2.Id);

            Assert.False(result1);
            Assert.True(result2);
            Assert.NotNull(detail1);
            Assert.NotNull(detail2);
            Assert.Equal("Update Group 1", detail1.Name);
            Assert.Equal("Update 2", detail2.Name);
            Assert.Equal(_actor.UserId, detail2.LastModifiedBy);

            cts.Dispose();
        }



        [Fact]
        public async Task BulkUpdateItemGroup_FailedDueToInvalidID()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemGroup1 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 1" };
            var itemGroup2 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 2" };
            dbContext.ItemGroups.Add(itemGroup1);
            dbContext.ItemGroups.Add(itemGroup2);

            var groupBulkUpload = new ItemGroupBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Update, UserId = _actor.UserId, CompanyId = _actor.CompanyId };
            dbContext.ItemGroupBulkUploads.Add(groupBulkUpload);

            var uploadDetail1 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Update Group 1" };
            var uploadDetail2 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup2.Id.ToString(), Column2 = "Update 2" };
            var uploadDetail3 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = "", Column2 = "Update 2" };
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail3);

            await dbContext.SaveAsync(_actor);

            var bulkUpdateService = new BulkUpdateGroupService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
            };


            var result1 = await bulkUpdateService.Save([], cts.Token);
            var result2 = await bulkUpdateService.Save(fieldMapping, cts.Token);
            var group1 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == itemGroup1.Id);
            var group2 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == itemGroup2.Id);
            var error1 = _langService.Format($"{resources}.ValueNotFoundInTheSystem.Message", uploadDetail1.Column1);
            var error2 = _langService.Format($"{resources}.KeyColumnIsEmpty.Message", groupBulkUpload.headerColum1);

            Assert.False(result1);
            Assert.False(result2);
            Assert.Equal("Item Group 1", group1!.Name);
            Assert.Equal("Item Group 2", group2!.Name);
            Assert.Contains(error1, uploadDetail1.Errors);
            Assert.Contains(error2, uploadDetail3.Errors);

            cts.Dispose();

        }
    }
}
