using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation
{
    public class ItemGroupBulkUpdate : UseDatabaseTestBase
    {
        [Fact]
        public async Task BulkUpdate_Submiting_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemGroup1 = new ItemGroup() {CompanyId = _actor.CompanyId, Name = "Item Group 1" };
            var itemGroup2 = new ItemGroup() {CompanyId = _actor.CompanyId, Name = "Item Group 2" };
            dbContext.ItemGroups.Add(itemGroup1);
            dbContext.ItemGroups.Add(itemGroup2);

            var groupBulkUpload = new ItemGroupBulkUpload() { 
                Id = Guid.NewGuid(), 
                headerColum1 = "Id", 
                headerColum2 = "Name", 
                Operation = CrudOperations.Update, 
                UserId = _actor.UserId, 
                CompanyId = _actor.CompanyId.Value };
            var uploadDetail1 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup1.Id.ToString(), Column2 = "Update 1" };
            var uploadDetail2 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup2.Id.ToString(), Column2 = "Update 2" };
            dbContext.ItemGroupBulkUploads.Add(groupBulkUpload);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail2);

            await dbContext.SaveAsync(_actor);

            IBulkMapping bulkUpdateService = new BulkMappingGroupUpdateService(_langService, dbContext, _actor);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
            };

            var keyColumn = new List<string>() { nameof(ItemGroupDto.Id) };
            var resultSave = await bulkUpdateService.Save(fieldMapping, keyColumn, cts.Token);
            var resultValidate = await bulkUpdateService.Validate(fieldMapping, keyColumn, cts.Token);
            var resultGroup1 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == itemGroup1.Id);
            var resultGroup2 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == itemGroup2.Id);
            var uploadResult = dbContext.ItemGroupBulkUploads.FirstOrDefault(x => x.Id == groupBulkUpload.Id);
            var uploadResultDetail = dbContext.ItemGroupBulkUploads.Any(x => x.Id == uploadDetail1.Id || x.Id == uploadDetail2.Id);

            Assert.True(resultSave);
            Assert.True(resultValidate);
            Assert.Equal("Update 1", resultGroup1!.Name);
            Assert.Equal("Update 2", resultGroup2!.Name);
            Assert.Null(uploadResult);
            Assert.False(uploadResultDetail);

            cts.Dispose();
        }
    }
}
