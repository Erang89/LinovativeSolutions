using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Item;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.Items
{
    public class ItemBulkDeleteTest : UseDatabaseTestBase
    {
        [Fact]
        public async Task BulkDeleteItem_Success()
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

            var item1 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 1", UnitId = unit.Id, CategoryId = category.Id, Code = "Code1", SellPrice = 10000 };
            var item2 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 2", UnitId = unit.Id, CategoryId = category.Id, Code = "Code2", SellPrice = 10000 };
            var item3 = new Item() { CompanyId = _actor.CompanyId, Name = "Item 3", UnitId = unit.Id, CategoryId = category.Id, Code = "Code3", SellPrice = 10000 };
            var item4 = new Item() { CompanyId = Guid.NewGuid(), Name = "Item 4", UnitId = unit.Id, CategoryId = category.Id, Code = "Code4", SellPrice = 10000 };
            dbContext.Items.Add(item1);
            dbContext.Items.Add(item2);
            dbContext.Items.Add(item3);
            dbContext.Items.Add(item4);

            var bulkUpload = new ItemBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", headerColum3 = "Unit", headerColum4 = "Category", headerColum5 = "Code", headerColum6 = "Sell Price", Operation = CrudOperations.Delete, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemBulkUploads.Add(bulkUpload);



            var uploadDetail1 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = item1.Id.ToString(), Column2 = "Update Item 1", Column3 = unit.Name, Column4 = category.Name, Column5 = "NewCode1", Column6 = "10000" };
            var uploadDetail2 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = item2.Id.ToString(), Column2 = "Update Item 2", Column3 = unit2.Name, Column4 = category2.Name, Column5 = "NewCode2", Column6 = "10000" };
            var uploadDetail3 = new ItemBulkUploadDetail() { ItemBulkUploadId = bulkUpload.Id, Column1 = item3.Id.ToString(), Column2 = "Item 4", Column3 = unit2.Name, Column4 = category2.Name, Column5 = "NewCode3", Column6 = "10000" };

            dbContext.ItemBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemBulkUploadDetails.Add(uploadDetail3);

            await dbContext.SaveAsync(_actor);


            var bulkService = new BulkDeleteItemService(dbContext, _actor, _langService);
            var fieldMapping = new Dictionary<string, string>()
            {
                {nameof(ItemDto.Id), nameof(ItemBulkUploadDetail.Column1) },
                {nameof(ItemDto.Name), nameof(ItemBulkUploadDetail.Column2) }
            };

            var saveResult = await bulkService.Save(fieldMapping, cts.Token);
            var uploadRow1 = dbContext.ItemBulkUploadDetails.FirstOrDefault(x => x.Id == uploadDetail1.Id);
            var uploadRow2 = dbContext.ItemBulkUploadDetails.FirstOrDefault(x => x.Id == uploadDetail2.Id);
            var uploadRow3 = dbContext.ItemBulkUploadDetails.FirstOrDefault(x => x.Id == uploadDetail3.Id);
            

            Assert.True(saveResult);
            Assert.Null(uploadRow1);
            Assert.Null(uploadRow2);
            Assert.Null(uploadRow3);
            Assert.True(item1.IsDeleted);
            Assert.True(item2.IsDeleted);
            Assert.True(item3.IsDeleted);

            cts.Dispose();
        }
    }
}
