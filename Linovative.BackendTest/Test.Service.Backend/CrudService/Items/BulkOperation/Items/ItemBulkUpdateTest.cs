using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Item;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;
using System.Linq;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.Items
{
    public class ItemBulkUpdateTest : UseDatabaseTestBase
    {

        const string resources = "BulkUploadCommand";


        [Fact]
        public async Task BulkUpdateItem_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var group = new ItemGroup() { Name = "Group", CompanyId = _actor.CompanyId };
            var group2 = new ItemGroup() { Name = "Group 2", CompanyId = _actor.CompanyId };
            dbContext.ItemGroups.AddRange(group, group2);
            var category = new ItemCategory() { Name = "Category", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category2 = new ItemCategory() { Name = "Category 2", GroupId = group.Id, CompanyId = _actor.CompanyId };
            dbContext.ItemCategories.AddRange(category, category2);
            var unit = new ItemUnit() { Name = "Unit", CompanyId = _actor.CompanyId };
            var unit2 = new ItemUnit() { Name = "Unit 2", CompanyId = _actor.CompanyId };
            dbContext.ItemUnits.AddRange(unit, unit2);

            //var item1 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 1", UnitId = unit.Id, CategoryId = category.Id, Code = "Code1", SellPrice = 10000 };
            //var item2 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 2", UnitId = unit.Id, CategoryId = category.Id, Code = "Code2", SellPrice = 10000 };
            //var item3 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 3", UnitId = unit.Id, CategoryId = category.Id, Code = "Code3", SellPrice = 10000 };
            //var item4 = new Item() { CompanyId = Guid.NewGuid(), Name = "Item 4", UnitId = unit.Id, CategoryId = category.Id, Code = "Code4", SellPrice = 10000 };

            var item1 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 1"};
            var item2 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 2"};
            var item3 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 3"};
            var item4 = new Item() { CompanyId = Guid.NewGuid(), Name = "Item 4"};

            dbContext.Items.Add(item1);
            dbContext.Items.Add(item2);
            dbContext.Items.Add(item3);
            dbContext.Items.Add(item4);

            var bulkUpload = new ItemBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", headerColum3 = "Unit", headerColum4 = "Category", headerColum5 = "Code", headerColum6 = "Sell Price", Operation = CrudOperations.Update, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = item1.Id.ToString(), Column2 = "Update Item 1", Column3 = unit.Name, Column4 = category.Name, Column5 = "NewCode1", Column6 = "10000" };
            var uploadDetail2 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = item2.Id.ToString(), Column2 = "Update Item 2", Column3 = unit2.Name, Column4 = category2.Name, Column5 = "NewCode2", Column6 = "10000" };
            var uploadDetail3 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = item3.Id.ToString(), Column2 = "Item 4", Column3 = unit2.Name, Column4 = category2.Name, Column5 = "NewCode3", Column6 = "10000" };

            dbContext.ItemBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail3);

            await dbContext.SaveAsync(_actor);


            var bulkService = new BulkUpdateItemService(dbContext, _actor, _langService);
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
            var updatedItem1 = dbContext.Items.FirstOrDefault(x => x.Name == uploadDetail1.Column2);
            var updatedItem2 = dbContext.Items.FirstOrDefault(x => x.Id == item2.Id);
            var updatedItem3 = dbContext.Items.FirstOrDefault(x => x.Id == item3.Id);
            var updatedItem4 = dbContext.Items.FirstOrDefault(x => x.Id == item4.Id);

            Assert.True(result1);
            Assert.NotNull(updatedItem1);
            Assert.NotNull(updatedItem2);
            Assert.NotNull(updatedItem3);
            Assert.NotNull(updatedItem4);

            Assert.Equal("Update Item 1", updatedItem1.Name);
            Assert.Equal("Update Item 2", updatedItem2.Name);
            Assert.Equal("Item 4", updatedItem3.Name);
            Assert.Equal("Item 4", updatedItem4.Name);

            Assert.Equal(_actor.UserId, updatedItem1.LastModifiedBy);
            //Assert.Equal(unit2.Id, updatedItem2.UnitId);
            //Assert.Equal(category2.Id, updatedItem2.CategoryId);

            cts.Dispose();

        }



        [Fact]
        public async Task BulkUpdateItem_Failed()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var group = new ItemGroup() { Name = "Group", CompanyId = _actor.CompanyId };
            var group2 = new ItemGroup() { Name = "Group 2", CompanyId = _actor.CompanyId };
            dbContext.ItemGroups.AddRange(group, group2);
            var category = new ItemCategory() { Name = "Category", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category2 = new ItemCategory() { Name = "Category 2", GroupId = group.Id, CompanyId = _actor.CompanyId };
            dbContext.ItemCategories.AddRange(category, category2);
            var unit = new ItemUnit() { Name = "Unit", CompanyId = _actor.CompanyId };
            var unit2 = new ItemUnit() { Name = "Unit 2", CompanyId = _actor.CompanyId };
            dbContext.ItemUnits.AddRange(unit, unit2);

            var item1 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 1"};
            var item2 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 2"};
            var item3 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 3"};
            var item4 = new Item() { CompanyId = Guid.NewGuid(), Name = "Item 4"};
            dbContext.Items.Add(item1);
            dbContext.Items.Add(item2);
            dbContext.Items.Add(item3);
            dbContext.Items.Add(item4);

            var bulkUpload = new ItemBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", headerColum3 = "Unit", headerColum4 = "Category", headerColum5 = "Code", headerColum6 = "Sell Price", Operation = CrudOperations.Update, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = "", Column2 = "", Column3 = unit.Name, Column4 = category.Name, Column5 = "NewCode1", Column6 = "xxx" };
            var uploadDetail2 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = item2.Id.ToString(), Column2 = "Item 3", Column3 = "Unit 3", Column4 = "Category 3", Column5 = "Code3", Column6 = "" };
            var uploadDetail3 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = item3.Id.ToString(), Column2 = "Item 4", Column3 = unit2.Name, Column4 = category2.Name, Column5 = "", Column6 = "-10000" };

            dbContext.ItemBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail3);

            await dbContext.SaveAsync(_actor);


            var bulkService = new BulkUpdateItemService(dbContext, _actor, _langService);
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
            var error1 = string.Format(_langService[$"{resources}.KeyColumnIsEmpty.Message"], bulkUpload.headerColum1);
            var error2 = string.Format(_langService[$"{resources}.ValueNotFoundInTheSystem.Message"], uploadDetail2.Column3);
            var error3 = string.Format(_langService[$"{resources}.ValueMustGreaterOrEqualThan.Message"], bulkUpload.headerColum6, "0");
            var error4 = string.Format(_langService[$"{resources}.ValueNotFoundInTheSystem.Message"], uploadDetail2.Column4);
            var error5 = string.Format(_langService[$"{resources}.ValueAlreadyExistInTheSystem.Message"], uploadDetail2.Column5);
            var error6 = string.Format(_langService[$"{resources}.ExcelFieldValueRequires.Message"], bulkUpload.headerColum6);
            var error7 = string.Format(_langService[$"{resources}.FailedToConvertAsDecimal.Message"], uploadDetail1.Column6);
            var error8 = string.Format(_langService[$"{resources}.ExcelFieldValueRequires.Message"], bulkUpload.headerColum2);

            Assert.False(result1);
            Assert.Contains(error1, uploadDetail1.Errors);
            Assert.Contains(error3, uploadDetail3.Errors);
            Assert.Contains(error2, uploadDetail2.Errors);
            Assert.Contains(error4, uploadDetail2.Errors);
            Assert.Contains(error5, uploadDetail2.Errors);
            Assert.Contains(error6, uploadDetail2.Errors);
            Assert.Contains(error7, uploadDetail1.Errors);
            Assert.Contains(error8, uploadDetail1.Errors);

            cts.Dispose();

        }
    }
}
