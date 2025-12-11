using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.ItemGroups
{
    public class ItemGroupBulkCreateTest : UseDatabaseTestBase
    {
        [Fact]
        public async Task BulkUpdate_Submiting_Success()
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
                Operation = CrudOperations.Create, 
                UserId = _actor.UserId, 
                CompanyId = _actor.CompanyId!.Value };
            var uploadDetail1 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup1.Id.ToString(), Column2 = "Item Group 1" };
            var uploadDetail2 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = groupBulkUpload.Id, Column1 = itemGroup2.Id.ToString(), Column2 = "Item Group 2" };
            dbContext.ItemGroupBulkUploads.Add(groupBulkUpload);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail2);

            await dbContext.SaveAsync(_actor);

            IBulkMapping bulkService = new BulkCreateItemGroupService(_langService, dbContext, _actor);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
            };

            var keyColumn = new List<string>() { nameof(ItemGroupDto.Id) };

            // ASERT GROUP 1
            var result1 = await bulkService.Validate(fieldMapping, keyColumn, cts.Token);
            var result2 = await bulkService.Validate(fieldMapping, new(), cts.Token);
            var expectedError1 = _langService.Format("BulkUploadCommand.DuplicateIdInDB.Message", "Id");
            Assert.False(result1);
            Assert.False(result2);
            Assert.Equal(expectedError1, uploadDetail1.Errors);
            Assert.Equal(expectedError1, uploadDetail2.Errors);


            // ASERT GROUP 2
            uploadDetail1.Column1 = Guid.NewGuid().ToString();
            uploadDetail2.Column1 = Guid.NewGuid().ToString();
            await dbContext.SaveAsync(_actor);
            var result3 = await bulkService.Validate(fieldMapping, new(), cts.Token);            
            var expectedError2 = _langService.Format("BulkUploadCommand.DuplicateNameInDB.Message", "Name");
            Assert.False(result3);
            Assert.Equal(expectedError2, uploadDetail1.Errors);
            Assert.Equal(expectedError2, uploadDetail2.Errors);


            // ASERT GROUP 3
            uploadDetail1.Column2 = "Test Create 2";
            uploadDetail2.Column2 = "test create 2";
            await dbContext.SaveAsync(_actor);
            var result4 = await bulkService.Validate(fieldMapping, new(), cts.Token);
            var expectedError3 = _langService.Format("BulkUploadCommand.DuplicateNameInExcel.Message", "Name");
            Assert.False(result4);
            Assert.Equal(expectedError3, uploadDetail1.Errors);
            Assert.Equal(expectedError3, uploadDetail2.Errors);


            // ASERT GROUP 4
            uploadDetail1.Column2 = null;
            uploadDetail2.Column2 = "test create 2";
            await dbContext.SaveAsync(_actor);
            var result5 = await bulkService.Validate(fieldMapping, new(), cts.Token);
            var expectedError5 = _langService.Format("BulkUploadCommand.GroupNameRequired.Message", "Name");
            Assert.False(result5);
            Assert.Equal(expectedError5, uploadDetail1.Errors);


            // ASERT GROUP 5
            uploadDetail1.Column2 = "Test Create 1";
            uploadDetail2.Column2 = "Test Create 2";
            await dbContext.SaveAsync(_actor);
            var result6 = await bulkService.Save(fieldMapping, new(), cts.Token);
            var newRow1 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == new Guid(uploadDetail1.Column1));
            var newRow2 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == new Guid(uploadDetail2.Column1));
            Assert.True(result6);
            Assert.NotNull(newRow1);
            Assert.NotNull(newRow2);
            Assert.Equal("Test Create 1", newRow1.Name);
            Assert.Equal("Test Create 2", newRow2.Name);


            cts.Dispose();
        }
    }
}
