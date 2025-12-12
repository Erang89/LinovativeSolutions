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
        public async Task BulkCreate_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemGroup1 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 1" };
            var itemGroup2 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 2" };
            var itemGroup3 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 3" };
            var itemGroup4 = new ItemGroup() { CompanyId = Guid.NewGuid(), Name = "Item Group 4" };
            dbContext.ItemGroups.Add(itemGroup1);
            dbContext.ItemGroups.Add(itemGroup2);
            dbContext.ItemGroups.Add(itemGroup3);
            dbContext.ItemGroups.Add(itemGroup4);

            var bulkUpload = new ItemGroupBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Create, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemGroupBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = bulkUpload.Id, Column1 = null, Column2 = "Create Group 1" };            
            var uploadDetail2 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Group 2" };
            var uploadDetail3 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Group 3" };
            var uploadDetail4 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Item Group 4" };
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail3);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail4);

            await dbContext.SaveAsync(_actor);


            IBulkOperationProcess bulkService = new BulkCreateGroupService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
            };


            // ASERT GROUP 1
            var validateResult = await bulkService.Validate(new() { { "Name", "Column2" } }, cts.Token);
            var validateResult2 = await bulkService.Validate(new() { { "xx", "Column2" } }, cts.Token);
            var validateResult3 = await bulkService.Validate(new() { { "Name", "xx" } }, cts.Token);
            var result1 = await bulkService.Save(fieldMapping, cts.Token);
            var resultDetail1 = dbContext.ItemGroups.FirstOrDefault(x => x.Name == "Create Group 1");
            var resultDetail2 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == new Guid(uploadDetail2.Column1!));
            var resultDetail3 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == new Guid(uploadDetail3.Column1!));
            var resultDetail4 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == new Guid(uploadDetail4.Column1!));
            var resultDetail5 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == itemGroup4.Id);

            Assert.True(validateResult);
            Assert.False(validateResult2);
            Assert.False(validateResult3);
            Assert.True(result1);
            Assert.NotNull(resultDetail1);
            Assert.NotNull(resultDetail2);
            Assert.NotNull(resultDetail3);
            Assert.NotNull(resultDetail4);
            Assert.NotNull(resultDetail5);
            Assert.Equal(uploadDetail1.Column2, resultDetail1.Name);
            Assert.Equal(uploadDetail2.Column2, resultDetail2.Name);
            Assert.Equal(uploadDetail3.Column2, resultDetail3.Name);
            Assert.Equal(uploadDetail4.Column2, resultDetail4.Name);
            Assert.Equal(itemGroup4.Name, resultDetail5.Name);
            Assert.NotEqual(itemGroup4.CompanyId, _actor.CompanyId);
            Assert.Equal(resultDetail1.CompanyId, _actor.CompanyId);
            Assert.Equal(resultDetail1.CreatedBy, _actor.UserId);


            cts.Dispose();
        }


        [Fact]
        public async Task BulkCreate_Failed_DueToDuplicateNameOrID()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();
            const string resources = "BulkUploadCommand";

            var itemGroup1 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 1" };
            var itemGroup2 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 2" };
            var itemGroup3 = new ItemGroup() { CompanyId = _actor.CompanyId, Name = "Item Group 3" };
            var itemGroup4 = new ItemGroup() { CompanyId = Guid.NewGuid(), Name = "Item Group 4" };
            dbContext.ItemGroups.Add(itemGroup1);
            dbContext.ItemGroups.Add(itemGroup2);
            dbContext.ItemGroups.Add(itemGroup3);
            dbContext.ItemGroups.Add(itemGroup4);

            var bulkUpload = new ItemGroupBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Create, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemGroupBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = bulkUpload.Id, Column1 = null, Column2 = "Create Group 1" };
            var uploadDetail2 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Item Group 2" };
            var uploadDetail3 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Group 3" };
            var uploadDetail4 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = bulkUpload.Id, Column1 = itemGroup4.Id.ToString(), Column2 = "Item Group 4" };
            var uploadDetail5 = new ItemGroupBulkUploadDetail() { ItemGroupBulkUploadId = bulkUpload.Id, Column1 = itemGroup4.Id.ToString(), Column2 = "" };
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail3);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail4);
            dbContext.ItemGroupBulkUploadDetails.Add(uploadDetail5);

            await dbContext.SaveAsync(_actor);


            IBulkOperationProcess bulkService = new BulkCreateGroupService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemGroupDto.Id), nameof(ItemGroupBulkUploadDetail.Column1) },
                {nameof(ItemGroupDto.Name), nameof(ItemGroupBulkUploadDetail.Column2) },
            };


            // ASERT GROUP 1
            var saveResult = await bulkService.Save(fieldMapping, cts.Token);
            var resultDetail1 = dbContext.ItemGroups.FirstOrDefault(x => x.Name == "Create Group 1");
            var resultDetail2 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == new Guid(uploadDetail2.Column1!));
            var resultDetail3 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == new Guid(uploadDetail3.Column1!));
            var resultDetail4 = dbContext.ItemGroups.FirstOrDefault(x => x.Id == new Guid(uploadDetail4.Column1!));
            var resultDetail5_Error = _langService.Format($"{resources}.ValueAlreadyExistInTheSystem.Message", itemGroup4.Id);
            var resultDetail2_Error = _langService.Format($"{resources}.ValueAlreadyExistInTheSystem.Message", uploadDetail2.Column2);
            var resultDetail6_Error = _langService.Format($"{resources}.ExcelFieldValueRequires.Message", bulkUpload.headerColum2);

            Assert.False(saveResult);
            Assert.Null(resultDetail1);
            Assert.Null(resultDetail2);
            Assert.Null(resultDetail3);
            Assert.NotNull(resultDetail4);
            Assert.Contains(resultDetail5_Error, uploadDetail4.Errors);
            Assert.Contains(resultDetail2_Error, uploadDetail2.Errors);
            Assert.Contains(resultDetail6_Error, uploadDetail5.Errors);

            var validateResult1 = await bulkService.Validate(new(), cts.Token);
            var validateResult2 = await bulkService.Validate(new() { { "Name", "Column2"} }, cts.Token);
            Assert.False(validateResult1);
            Assert.False(validateResult2);

            cts.Dispose();
        }
    }
}
