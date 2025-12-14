using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Category;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using Microsoft.EntityFrameworkCore;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.ItemCategories
{
    public class BulkCreateItemCategoryTest : UseDatabaseTestBase
    {
        const string resources = "BulkUploadCommand";

        [Fact]
        public async Task CreateCategory_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var group = new ItemGroup() { Name = "Group 1", CompanyId = _actor.CompanyId };
            var otherGroup = new ItemGroup() { Name = "Group 1", CompanyId = _actor.CompanyId };
            dbContext.ItemGroups.AddRange(group, otherGroup);

            var category1 = new ItemCategory() { Name = "Category 1", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category2 = new ItemCategory() { Name = "Category 2", GroupId = group.Id, CompanyId = _actor.CompanyId};
            var category3 = new ItemCategory() { Name = "Category 3", GroupId = otherGroup.Id, CompanyId = Guid.NewGuid() };
            dbContext.ItemCategories.AddRange(category1, category2, category3);
            

            var bulkUpload = new ItemCategoryBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Create, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemCategoryBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = null, Column2 = "Create Category 1", Column3 = "Group 1" };
            var uploadDetail2 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Category 2", Column3 = "Group 1" };
            var uploadDetail3 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Category 3", Column3 = "Group 1" };
            var uploadDetail4 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Category 3", Column3 = "Group 1" };

            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail1);     
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail3);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail4);

            await dbContext.SaveAsync(_actor);

            var service = new BulkCreateCategoryService(dbContext, _actor, _langService);
            var mapping = new Dictionary<string, string>()
            {
                {"Id", "Column1" },
                {"Name", "Column2" },
                {"Group", "Column3" }
            };

            var result = await service.Save(mapping, cts.Token);
            var newCategory1 = await dbContext.ItemCategories.FirstOrDefaultAsync(x => x.Name == "Create Category 1");
            var newCategory2 = await dbContext.ItemCategories.FirstOrDefaultAsync(x => x.Id == new Guid(uploadDetail2.Column1!));
            var newCategory3 = await dbContext.ItemCategories.FirstOrDefaultAsync(x => x.Id == new Guid(uploadDetail3.Column1!));
            var newCategory4 = await dbContext.ItemCategories.FirstOrDefaultAsync(x => x.Id == new Guid(uploadDetail4.Column1!));
            var otherCategory = await dbContext.ItemCategories.FirstOrDefaultAsync(x => x.Id == category3.Id);

            Assert.True(result);
            Assert.NotNull(newCategory1);
            Assert.NotNull(newCategory2);
            Assert.NotNull(newCategory3);
            Assert.NotNull(newCategory4);
            Assert.NotNull(newCategory4);

            Assert.Equal(uploadDetail1.Column2, newCategory1.Name);
            Assert.Equal(uploadDetail2.Column2, newCategory2.Name);
            Assert.Equal(uploadDetail3.Column2, newCategory3.Name);
            Assert.Equal(uploadDetail4.Column2, newCategory4.Name);
            Assert.Equal("Category 3", otherCategory!.Name);

            Assert.Equal(group.Id, newCategory1.GroupId);
            Assert.Equal(_actor.UserId, newCategory1.CreatedBy);
            Assert.Equal(_actor.CompanyId, newCategory1.CompanyId);

            cts.Dispose();
        }



        [Fact]
        public async Task CreateCategory_FailedDueToCategoryAlreadyExist()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var group = new ItemGroup() { Name = "Group 1", CompanyId = _actor.CompanyId };
            var otherGroup = new ItemGroup() { Name = "Group 1", CompanyId = _actor.CompanyId };
            dbContext.ItemGroups.AddRange(group, otherGroup);

            var category1 = new ItemCategory() { Name = "Category 1", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category2 = new ItemCategory() { Name = "Category 2", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category3 = new ItemCategory() { Name = "Category 3", GroupId = otherGroup.Id, CompanyId = Guid.NewGuid() };
            dbContext.ItemCategories.AddRange(category1, category2, category3);


            var bulkUpload = new ItemCategoryBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", headerColum3 = "Category", Operation = CrudOperations.Create, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemCategoryBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = null, Column2 = "Category 1", Column3 = "Group 1" };
            var uploadDetail2 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "", Column3 = "Group 1" };
            var uploadDetail3 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Create Category 3", Column3 = "" };
            var uploadDetail4 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Category 3", Column3 = "Group 1" };

            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail3);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail4);

            await dbContext.SaveAsync(_actor);

            var service = new BulkCreateCategoryService(dbContext, _actor, _langService);
            var mapping = new Dictionary<string, string>()
            {
                {"Id", "Column1" },
                {"Name", "Column2" },
                {"Group", "Column3" }
            };

            var result = await service.Save(mapping, cts.Token);
            var error1 = _langService.Format($"{resources}.ValueAlreadyExistInTheSystem.Message", uploadDetail1.Column2);
            var error2 = _langService.Format($"{resources}.ExcelFieldValueRequires.Message", bulkUpload.headerColum2);
            var error3 = _langService.Format($"{resources}.ExcelFieldValueRequires.Message", bulkUpload.headerColum3);

            Assert.False(result);
            Assert.Contains(error1, uploadDetail1.Errors);
            Assert.Contains(error2, uploadDetail2.Errors);
            Assert.Contains(error3, uploadDetail3.Errors);

            cts.Dispose();
        }
    }
}
