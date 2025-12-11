using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation
{
    public class ItemGroupBulkDeleteTest : UseDatabaseTestBase
    {
        [Fact]
        public async Task BulkDelete_Submiting_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemGroup1 = new ItemGroup() {CompanyId = _actor.CompanyId, Name = "Item Group 1" };
            var itemGroup2 = new ItemGroup() {CompanyId = _actor.CompanyId, Name = "Item Group 2" };
            var itemGroup3 = new ItemGroup() {CompanyId = _actor.CompanyId, Name = "Item Group 3" };
            var itemGroup4 = new ItemGroup() {CompanyId = Guid.NewGuid(), Name = "Other Company Item Group" };
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
            var itemGroupResult1 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == itemGroup1.Id);
            var itemGroupResult2 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == itemGroup2.Id);

            Assert.True(saveResult);
            Assert.NotNull(itemGroupResult1);
            Assert.NotNull(itemGroupResult2);
            Assert.True(itemGroupResult1.IsDeleted);
            Assert.True(itemGroupResult2.IsDeleted);

            cts.Dispose();
        }
    }
}
