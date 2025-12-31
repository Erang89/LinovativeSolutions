using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Item;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.Items
{
    public class ItemBulkCreateTest : UseDatabaseTestBase
    {

        const string resources = "BulkUploadCommand";


        [Fact]
        public async Task CreateItem_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var group = new ItemGroup() { Name = "Group", CompanyId = _actor.CompanyId };
            dbContext.ItemGroups.Add(group);
            var category = new ItemCategory() { Name = "Category", GroupId = group.Id, CompanyId = _actor.CompanyId };
            dbContext.ItemCategories.Add(category);
            var unit = new ItemUnit() { Name = "Unit", CompanyId = _actor.CompanyId };
            dbContext.ItemUnits.Add(unit);

            var item1 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 1", UnitId = unit.Id, CategoryId = category.Id, Code = "Code1", SellPrice = 10000};
            var item2 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 2", UnitId = unit.Id, CategoryId = category.Id, Code = "Code2", SellPrice = 10000 };
            var item3 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 3", UnitId = unit.Id, CategoryId = category.Id, Code = "Code3", SellPrice = 10000 };
            var item4 = new Item() { CompanyId = Guid.NewGuid(), Name = "Item 4", UnitId = unit.Id, CategoryId = category.Id, Code = "Code4", SellPrice = 10000 };
            dbContext.Items.Add(item1);
            dbContext.Items.Add(item2);
            dbContext.Items.Add(item3);
            dbContext.Items.Add(item4);

            var bulkUpload = new ItemBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", headerColum3 = "Unit", headerColum4 = "Category", headerColum5 = "Code", headerColum6 = "Sell Price", Operation = CrudOperations.Create, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = null, Column2 = "Create Item 1", Column3 = unit.Name, Column4 = category.Name, Column5 = "NewCode1", Column6 = "10000" };
            var uploadDetail2 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Item 2", Column3 = unit.Name, Column4 = category.Name, Column5 = "NewCode2", Column6 = "10000" };
            var uploadDetail3 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Item 3", Column3 = unit.Name, Column4 = category.Name, Column5 = "NewCode3", Column6 = "10000" };
            var uploadDetail4 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Item Item 4", Column3 = unit.Name, Column4 = category.Name, Column5 = "NewCode4", Column6 = "10000" };
            dbContext.ItemBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail3);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail4);

            await dbContext.SaveAsync(_actor);


            var bulkService = new BulkCreateItemService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemDto.Id), nameof(ItemBulkUploadDetail.Column1) },
                {nameof(ItemDto.Name), nameof(ItemBulkUploadDetail.Column2) },
                {"Unit", nameof(ItemBulkUploadDetail.Column3) },
                {"Category", nameof(ItemBulkUploadDetail.Column4) },
                {"Code", nameof(ItemBulkUploadDetail.Column5) },
                {"SellPrice", nameof(ItemBulkUploadDetail.Column6) },
            };

            var result1 = await bulkService.Save(fieldMapping, cts.Token);
            var resultDetail1 = dbContext.Items.FirstOrDefault(x => x.Name == "Create Item 1");
            var resultDetail2 = dbContext.Items.FirstOrDefault(x => x.Id == new Guid(uploadDetail2.Column1!));
            var resultDetail3 = dbContext.Items.FirstOrDefault(x => x.Id == new Guid(uploadDetail3.Column1!));
            var resultDetail4 = dbContext.Items.FirstOrDefault(x => x.Id == new Guid(uploadDetail4.Column1!));
            var resultDetail5 = dbContext.Items.FirstOrDefault(x => x.Id == item4.Id);

            Assert.True(result1);
            Assert.NotNull(resultDetail1);
            Assert.NotNull(resultDetail2);
            Assert.NotNull(resultDetail3);
            Assert.NotNull(resultDetail4);
            Assert.NotNull(resultDetail5);

            Assert.Equal(unit.Id, resultDetail1.UnitId);
            Assert.Equal(unit.Id, resultDetail2.UnitId);
            Assert.Equal(unit.Id, resultDetail3.UnitId);
            Assert.Equal(unit.Id, resultDetail4.UnitId);
            Assert.Equal(unit.Id, resultDetail5.UnitId);

            Assert.Equal(category.Id, resultDetail1.CategoryId);
            Assert.Equal(category.Id, resultDetail2.CategoryId);
            Assert.Equal(category.Id, resultDetail3.CategoryId);
            Assert.Equal(category.Id, resultDetail4.CategoryId);
            Assert.Equal(category.Id, resultDetail5.CategoryId);

            Assert.Equal(uploadDetail1.Column2, resultDetail1.Name);
            Assert.Equal(uploadDetail2.Column2, resultDetail2.Name);
            Assert.Equal(uploadDetail3.Column2, resultDetail3.Name);
            Assert.Equal(uploadDetail4.Column2, resultDetail4.Name);

            Assert.Equal(item4.Name, resultDetail5.Name);
            Assert.NotEqual(item4.CompanyId, _actor.CompanyId);
            Assert.Equal(resultDetail1.CompanyId, _actor.CompanyId);
            Assert.Equal(resultDetail1.CreatedBy, _actor.UserId);

            cts.Dispose();
        }

        [Fact]
        public async Task CreateItem_Failed()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();


            var bulkUpload = new ItemBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", headerColum3 = "Unit", headerColum4 = "Category", headerColum5 = "Code", headerColum6 = "Sell Price", Operation = CrudOperations.Create, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = null, Column2 = "Create Item 1", Column3 = "Unit", Column4 = "Category", Column5 = "NewCode1", Column6 = "-1" };
            var uploadDetail2 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Item 2", Column3 = "", Column4 = null, Column5 = "NewCode2", Column6 = "10000" };
            var uploadDetail3 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Item 3", Column3 = "Unit", Column4 = "Category", Column5 = "NewCode3", Column6 = "xx" };
            var uploadDetail4 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Item Item 4", Column3 = "Unit", Column4 = "Category", Column5 = "NewCode4", Column6 = "10000" };
            dbContext.ItemBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail3);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail4);


            await dbContext.SaveAsync(_actor);


            var bulkService = new BulkCreateItemService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemDto.Id), nameof(ItemBulkUploadDetail.Column1) },
                {nameof(ItemDto.Name), nameof(ItemBulkUploadDetail.Column2) },
                {"Unit", nameof(ItemBulkUploadDetail.Column3) },
                {"Category", nameof(ItemBulkUploadDetail.Column4) },
                {"Code", nameof(ItemBulkUploadDetail.Column5) },
                {"SellPrice", nameof(ItemBulkUploadDetail.Column6) },
            };


            var result1 = await bulkService.Save(fieldMapping, cts.Token);
            var error1 = string.Format(_langService[$"{resources}.ValueMustGreaterOrEqualThan.Message"], bulkUpload.headerColum6, "0");
            var error2 = string.Format(_langService[$"{resources}.ValueNotFoundInTheSystem.Message"], uploadDetail1.Column3);
            var error3 = string.Format(_langService[$"{resources}.ValueNotFoundInTheSystem.Message"], uploadDetail1.Column4);
            var error4 = string.Format(_langService[$"{resources}.ExcelFieldValueRequires.Message"], uploadDetail1.Column3);
            var error5 = string.Format(_langService[$"{resources}.ExcelFieldValueRequires.Message"], uploadDetail1.Column4);
            var error6 = string.Format(_langService[$"{resources}.FailedToConvertAsDecimal.Message"], uploadDetail3.Column6);
           
            Assert.False(result1);
            Assert.Contains(error1, uploadDetail1.Errors);
            Assert.Contains(error2, uploadDetail1.Errors);
            Assert.Contains(error3, uploadDetail1.Errors);
            Assert.Contains(error4, uploadDetail2.Errors);
            Assert.Contains(error5, uploadDetail2.Errors);
            Assert.Contains(error6, uploadDetail3.Errors);

            fieldMapping.Remove("Unit");
            var result = await bulkService.Save(fieldMapping, cts.Token);
            var fields = new List<string>() { "Name", "Code", "Unit", "Category", "SellPrice" };
            var requiredFields = string.Join(", ", fields.Select(x => string.Format(_langService[$"{resources}.{x}.ColumnHeader"], x)));
            var error7 = string.Format(_langService[$"{resources}.FieldsRequired.Message"], requiredFields);
            Assert.False(result);
            Assert.Contains(error7, result.Message);

            cts.Dispose();
        }
    }
}
