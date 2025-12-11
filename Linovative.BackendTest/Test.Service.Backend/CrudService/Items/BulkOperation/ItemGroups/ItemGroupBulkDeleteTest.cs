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
        [Fact]
        public async Task BulkDelete_Submiting_Success()
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

            IBulkMapping bulkService = new BulkDeleteGroupService(_langService, dbContext, _actor);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
            };

            var keyColumn = new List<string>() { nameof(ItemGroupDto.Id) };
            var saveResult = await bulkService.Save(fieldMapping, keyColumn, cts.Token);
            ItemGroup? itemGroupResult1 = await dbContext.ItemGroups.Where(x => x.Id == itemGroup1.Id).FirstOrDefaultAsync();
            ItemGroup? itemGroupResult2 = await dbContext.ItemGroups.Where(x => x.Id == itemGroup2.Id).FirstOrDefaultAsync();

            Assert.True(saveResult);
            Assert.NotNull(itemGroupResult1);
            Assert.NotNull(itemGroupResult2);
            Assert.True(itemGroupResult1.IsDeleted);
            Assert.True(itemGroupResult2.IsDeleted);

            cts.Dispose();
        }


        [Fact]
        public async Task BulkDelete_InvalidKeyColumn_Submiting_Faild()
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
            var uploadDetail1 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup1.Id.ToString(), Column2 = "Item Group 1" };
            var uploadDetail2 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup2.Id.ToString(), Column2 = "Item Group 2" };
            dbContext.ItemGroupBulkUploads.Add(groupBulkUpload);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail2);

            await dbContext.SaveAsync(_actor);


            IBulkMapping bulkService = new BulkDeleteGroupService(_langService, dbContext, _actor);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
            };

            var fieldMapping2 = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
                {"xx", nameof(ItemGroupBulkUploadDetail.Column2) },
            };

            var fieldMapping3 = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column2) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column1) },
            };

            var keyColumn = new List<string>() { "Id" };
            var keyColumn2 = new List<string>() { "xx" };
            var saveResult = await bulkService.Save(fieldMapping, new(), cts.Token);
            var saveResult2 = await bulkService.Save(fieldMapping, keyColumn2, cts.Token);
            var saveResult3 = await bulkService.Save(fieldMapping2, keyColumn, cts.Token);
            var saveResult4 = await bulkService.Save(fieldMapping3, keyColumn, cts.Token);
            var saveResult5 = await bulkService.Validate(fieldMapping, keyColumn, cts.Token);

            Assert.False(saveResult);
            Assert.False(saveResult2);
            Assert.False(saveResult3);
            Assert.False(saveResult4);
            Assert.True(saveResult5);

            cts.Dispose();
        }
    }
}
