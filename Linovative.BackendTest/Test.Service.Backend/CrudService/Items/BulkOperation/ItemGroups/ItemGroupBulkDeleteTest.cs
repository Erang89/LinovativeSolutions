using DocumentFormat.OpenXml.InkML;
using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.EntityFrameworkCore;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.ItemGroups
{
    public class ItemGroupBulkDeleteTest : UseDatabaseTestBase
    {
        const string resources = "BulkUploadCommand";

        [Fact]
        public async Task BulkDeleteItemGroup_Submiting_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemGroup1 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 1" };
            var itemGroup2 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 2" };
            var itemGroup3 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 3" };
            var itemGroup4 = new ItemGroup() { CompanyId = Guid.NewGuid(), Name = "Other Company Item Group" };
            dbContext.ItemGroups.Add(itemGroup1);
            dbContext.ItemGroups.Add(itemGroup2);
            dbContext.ItemGroups.Add(itemGroup3);
            dbContext.ItemGroups.Add(itemGroup4);

            var groupBulkUpload = new ItemGroupBulkUpload() {
                Id = Guid.NewGuid(),
                headerColum1 = "Id",
                headerColum2 = "Name",
                Operation = CrudOperations.Delete,
                UserId = _actor.UserId,
                CompanyId = _actor.CompanyId!.Value };
            var uploadDetail1 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup1.Id.ToString(), Column2 = "Item Group 1" };
            var uploadDetail2 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup2.Id.ToString(), Column2 = "Item Group 2" };
            dbContext.ItemGroupBulkUploads.Add(groupBulkUpload);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail2);

            await dbContext.SaveAsync(_actor);

            var bulkService = new BulkDeleteItemGroupService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
            };

            var saveResult = await bulkService.Save(fieldMapping, cts.Token);
            ItemGroup? itemGroupResult1 = await dbContext.ItemGroups.Where(x => x.Id == itemGroup1.Id).FirstOrDefaultAsync();
            ItemGroup? itemGroupResult2 = await dbContext.ItemGroups.Where(x => x.Id == itemGroup2.Id).FirstOrDefaultAsync();

            Assert.True(saveResult);
            Assert.NotNull(itemGroupResult1);
            Assert.NotNull(itemGroupResult2);
            Assert.True(itemGroupResult1.IsDeleted);
            Assert.True(itemGroupResult2.IsDeleted);
            Assert.Equal(_actor.UserId, itemGroupResult2.LastModifiedBy);

            cts.Dispose();
        }




        [Fact]
        public async Task BulkDeleteItemGroup_Failed_DueToInCorectID()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemGroup1 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 1" };
            var itemGroup2 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 2" };
            var itemGroup3 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 3" };
            var itemGroup4 = new ItemGroup() { CompanyId = Guid.NewGuid(), Name = "Other Company Item Group" };
            dbContext.ItemGroups.Add(itemGroup1);
            dbContext.ItemGroups.Add(itemGroup2);
            dbContext.ItemGroups.Add(itemGroup3);
            dbContext.ItemGroups.Add(itemGroup4);

            var groupBulkUpload = new ItemGroupBulkUpload()
            {
                Id = Guid.NewGuid(),
                headerColum1 = "Id",
                headerColum2 = "Name",
                Operation = CrudOperations.Delete,
                UserId = _actor.UserId,
                CompanyId = _actor.CompanyId!.Value
            };
            var uploadDetail1 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Item Group 1" };
            var uploadDetail2 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup2.Id.ToString(), Column2 = "Item Group 2" };
            dbContext.ItemGroupBulkUploads.Add(groupBulkUpload);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail2);

            await dbContext.SaveAsync(_actor);

            var bulkService = new BulkDeleteItemGroupService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
            };

            var saveResult = await bulkService.Save(fieldMapping, cts.Token);
            var errorMessage1 = _langService.Format($"{resources}.ValueNotFoundInTheSystem.Message", uploadDetail1.Column1);
            ItemGroup? itemGroupResult1 = await dbContext.ItemGroups.Where(x => x.Id == itemGroup1.Id).FirstOrDefaultAsync();
            ItemGroup? itemGroupResult2 = await dbContext.ItemGroups.Where(x => x.Id == itemGroup2.Id).FirstOrDefaultAsync();

            Assert.False(saveResult);
            Assert.NotNull(itemGroupResult1);
            Assert.NotNull(itemGroupResult2);
            Assert.False(itemGroupResult1.IsDeleted);
            Assert.False(itemGroupResult2.IsDeleted);
            Assert.Contains(errorMessage1, uploadDetail1.Errors);

            cts.Dispose();
        }


        [Fact]
        public async Task BulkDeleteItemGroup_FailedDueToNoKeyMapping()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var group1 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 1" };
            dbContext.ItemGroups.Add(group1);

            var upload = new ItemGroupBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Delete, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value};
            var row1 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = upload.Id, Column1 = group1.Id.ToString(), Column2 = "Item Group 1" };
            dbContext.ItemGroupBulkUploads.Add(upload);
            dbContext.ItemGroupBulkUploadDetails.Add(row1);
            await dbContext.SaveAsync(_actor);

            var bulkService = new BulkDeleteItemGroupService(dbContext, _actor, _langService);
            var result = await bulkService.Save([], cts.Token);
            var result2 = await bulkService.Save(new() {{ "xx", "xx" }}, cts.Token);
            var result3 = await bulkService.Save(new() {{ "Name", "Column2" }}, cts.Token);
            var result4 = await bulkService.Save(new() {{ "Id", "Column2" }}, cts.Token);

            Assert.False(result);
            Assert.False(result2);
            Assert.False(result3);
            Assert.False(result4);

            cts.Dispose();
        }
    }
}
