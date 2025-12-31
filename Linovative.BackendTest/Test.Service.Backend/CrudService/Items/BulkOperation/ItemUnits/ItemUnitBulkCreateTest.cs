using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Group;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Unit;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.ItemUnits
{
    public class ItemUnitBulkCreateTest : UseDatabaseTestBase
    {
        [Fact]
        public async Task BulkCreateItemUnit_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemUnit1 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 1" };
            var itemUnit2 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 2" };
            var itemUnit3 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 3" };
            var itemUnit4 = new ItemUnit() { CompanyId = Guid.NewGuid(), Name = "Item Unit 4" };
            dbContext.ItemUnits.Add(itemUnit1);
            dbContext.ItemUnits.Add(itemUnit2);
            dbContext.ItemUnits.Add(itemUnit3);
            dbContext.ItemUnits.Add(itemUnit4);

            var bulkUpload = new ItemUnitBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Create, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemUnitBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = bulkUpload.Id, Column1 = null, Column2 = "Create Unit 1" };            
            var uploadDetail2 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Unit 2" };
            var uploadDetail3 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Unit 3" };
            var uploadDetail4 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Item Unit 4" };
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail3);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail4);

            await dbContext.SaveAsync(_actor);


            var bulkService = new BulkCreateUnitService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemUnitDto.Id), nameof(ItemUnitBulkUploadDetail.Column1) },
                {nameof(ItemUnitDto.Name), nameof(ItemUnitBulkUploadDetail.Column2) },
            };


            var validateResult = await bulkService.Validate(new() { { "Name", "Column2" } }, cts.Token);
            var validateResult2 = await bulkService.Validate(new() { { "xx", "Column2" } }, cts.Token);
            var validateResult3 = await bulkService.Validate(new() { { "Name", "xx" } }, cts.Token);
            var result1 = await bulkService.Save(fieldMapping, cts.Token);
            var resultDetail1 = dbContext.ItemUnits.FirstOrDefault(x => x.Name == "Create Unit 1");
            var resultDetail2 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == new Guid(uploadDetail2.Column1!));
            var resultDetail3 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == new Guid(uploadDetail3.Column1!));
            var resultDetail4 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == new Guid(uploadDetail4.Column1!));
            var resultDetail5 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == itemUnit4.Id);

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
            Assert.Equal(itemUnit4.Name, resultDetail5.Name);
            Assert.NotEqual(itemUnit4.CompanyId, _actor.CompanyId);
            Assert.Equal(resultDetail1.CompanyId, _actor.CompanyId);
            Assert.Equal(resultDetail1.CreatedBy, _actor.UserId);


            cts.Dispose();
        }


        [Fact]
        public async Task BulkCreateItemUnit_Failed_DueToDuplicateNameOrID()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();
            const string resources = "BulkUploadCommand";

            var itemUnit1 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 1" };
            var itemUnit2 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 2" };
            var itemUnit3 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 3" };
            var itemUnit4 = new ItemUnit() { CompanyId = Guid.NewGuid(), Name = "Item Unit 4" };
            dbContext.ItemUnits.Add(itemUnit1);
            dbContext.ItemUnits.Add(itemUnit2);
            dbContext.ItemUnits.Add(itemUnit3);
            dbContext.ItemUnits.Add(itemUnit4);

            var bulkUpload = new ItemUnitBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Create, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemUnitBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = bulkUpload.Id, Column1 = null, Column2 = "Create Unit 1" };
            var uploadDetail2 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Item Unit 2" };
            var uploadDetail3 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Unit 3" };
            var uploadDetail4 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = bulkUpload.Id, Column1 = itemUnit4.Id.ToString(), Column2 = "Item Unit 4" };
            var uploadDetail5 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = bulkUpload.Id, Column1 = itemUnit4.Id.ToString(), Column2 = "" };
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail3);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail4);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail5);

            await dbContext.SaveAsync(_actor);


            var bulkService = new BulkCreateUnitService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemUnitDto.Id), nameof(ItemUnitBulkUploadDetail.Column1) },
                {nameof(ItemUnitDto.Name), nameof(ItemUnitBulkUploadDetail.Column2) },
            };


            var saveResult = await bulkService.Save(fieldMapping, cts.Token);
            var resultDetail1 = dbContext.ItemUnits.FirstOrDefault(x => x.Name == "Create Unit 1");
            var resultDetail2 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == new Guid(uploadDetail2.Column1!));
            var resultDetail3 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == new Guid(uploadDetail3.Column1!));
            var resultDetail4 = dbContext.ItemUnits.FirstOrDefault(x => x.Id == new Guid(uploadDetail4.Column1!));
            var resultDetail5_Error = _langService.Format($"{resources}.ValueAlreadyExistInTheSystem.Message", itemUnit4.Id);
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

            var validateResult1 = await bulkService.Validate([], cts.Token);
            var validateResult2 = await bulkService.Validate(new() { { "Name", "Column2"} }, cts.Token);
            Assert.False(validateResult1);
            Assert.False(validateResult2);

            cts.Dispose();
        }
    }
}
