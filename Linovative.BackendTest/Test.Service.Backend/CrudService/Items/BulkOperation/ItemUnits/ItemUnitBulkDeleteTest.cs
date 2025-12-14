using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Unit;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.EntityFrameworkCore;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.ItemUnits
{
    public class ItemUnitBulkDeleteTest : UseDatabaseTestBase
    {
        const string resources = "BulkUploadCommand";

        [Fact]
        public async Task BulkDeleteItemUnit_Submiting_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemUnit1 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 1" };
            var itemUnit2 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 2" };
            var itemUnit3 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 3" };
            var itemUnit4 = new ItemUnit() { CompanyId = Guid.NewGuid(), Name = "Other Company Item Unit" };
            dbContext.ItemUnits.Add(itemUnit1);
            dbContext.ItemUnits.Add(itemUnit2);
            dbContext.ItemUnits.Add(itemUnit3);
            dbContext.ItemUnits.Add(itemUnit4);

            var UnitBulkUpload = new ItemUnitBulkUpload() {
                Id = Guid.NewGuid(),
                headerColum1 = "Id",
                headerColum2 = "Name",
                Operation = CrudOperations.Delete,
                UserId = _actor.UserId,
                CompanyId = _actor.CompanyId!.Value };
            var uploadDetail1 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = UnitBulkUpload.Id, Column1 = itemUnit1.Id.ToString(), Column2 = "Item Unit 1" };
            var uploadDetail2 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = UnitBulkUpload.Id, Column1 = itemUnit2.Id.ToString(), Column2 = "Item Unit 2" };
            dbContext.ItemUnitBulkUploads.Add(UnitBulkUpload);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail2);

            await dbContext.SaveAsync(_actor);

            var bulkService = new BulkDeleteUnitService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemUnitDto.Id), nameof(ItemUnitBulkUploadDetail.Column1) },
                {nameof(ItemUnitDto.Name), nameof(ItemUnitBulkUploadDetail.Column2) },
            };

            var saveResult = await bulkService.Save(fieldMapping, cts.Token);
            ItemUnit? itemUnitResult1 = await dbContext.ItemUnits.Where(x => x.Id == itemUnit1.Id).FirstOrDefaultAsync();
            ItemUnit? itemUnitResult2 = await dbContext.ItemUnits.Where(x => x.Id == itemUnit2.Id).FirstOrDefaultAsync();

            Assert.True(saveResult);
            Assert.NotNull(itemUnitResult1);
            Assert.NotNull(itemUnitResult2);
            Assert.True(itemUnitResult1.IsDeleted);
            Assert.True(itemUnitResult2.IsDeleted);
            Assert.Equal(_actor.UserId, itemUnitResult2.LastModifiedBy);

            cts.Dispose();
        }




        [Fact]
        public async Task BulkDeleteItemUnit_Failed_DueToInCorectID()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var itemUnit1 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 1" };
            var itemUnit2 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 2" };
            var itemUnit3 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 3" };
            var itemUnit4 = new ItemUnit() { CompanyId = Guid.NewGuid(), Name = "Other Company Item Unit" };
            dbContext.ItemUnits.Add(itemUnit1);
            dbContext.ItemUnits.Add(itemUnit2);
            dbContext.ItemUnits.Add(itemUnit3);
            dbContext.ItemUnits.Add(itemUnit4);

            var UnitBulkUpload = new ItemUnitBulkUpload()
            {
                Id = Guid.NewGuid(),
                headerColum1 = "Id",
                headerColum2 = "Name",
                Operation = CrudOperations.Delete,
                UserId = _actor.UserId,
                CompanyId = _actor.CompanyId!.Value
            };
            var uploadDetail1 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = UnitBulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Item Unit 1" };
            var uploadDetail2 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = UnitBulkUpload.Id, Column1 = itemUnit2.Id.ToString(), Column2 = "Item Unit 2" };
            dbContext.ItemUnitBulkUploads.Add(UnitBulkUpload);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemUnitBulkUploadDetails.Add(uploadDetail2);

            await dbContext.SaveAsync(_actor);

            var bulkService = new BulkDeleteUnitService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemUnitDto.Id), nameof(ItemUnitBulkUploadDetail.Column1) },
                {nameof(ItemUnitDto.Name), nameof(ItemUnitBulkUploadDetail.Column2) },
            };

            var saveResult = await bulkService.Save(fieldMapping, cts.Token);
            var errorMessage1 = _langService.Format($"{resources}.ValueNotFoundInTheSystem.Message", uploadDetail1.Column1);
            ItemUnit? itemUnitResult1 = await dbContext.ItemUnits.Where(x => x.Id == itemUnit1.Id).FirstOrDefaultAsync();
            ItemUnit? itemUnitResult2 = await dbContext.ItemUnits.Where(x => x.Id == itemUnit2.Id).FirstOrDefaultAsync();

            Assert.False(saveResult);
            Assert.NotNull(itemUnitResult1);
            Assert.NotNull(itemUnitResult2);
            Assert.False(itemUnitResult1.IsDeleted);
            Assert.False(itemUnitResult2.IsDeleted);
            Assert.Contains(errorMessage1, uploadDetail1.Errors);

            cts.Dispose();
        }


        [Fact]
        public async Task BulkDeleteItemUnit_FailedDueToNoKeyMapping()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var Unit1 = new ItemUnit() { CompanyId = _actor.CompanyId, Name = "Item Unit 1" };
            dbContext.ItemUnits.Add(Unit1);

            var upload = new ItemUnitBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Delete, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value};
            var row1 = new ItemUnitBulkUploadDetail() { ItemUnitBulkUploadId = upload.Id, Column1 = Unit1.Id.ToString(), Column2 = "Item Unit 1" };
            dbContext.ItemUnitBulkUploads.Add(upload);
            dbContext.ItemUnitBulkUploadDetails.Add(row1);
            await dbContext.SaveAsync(_actor);

            var bulkService = new BulkDeleteUnitService(dbContext, _actor, _langService);
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
