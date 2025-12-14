using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Category;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.ItemCategories
{
    public class BulkUpdateCategoryTest : UseDatabaseTestBase
    {
        const string resources = "BulkUploadCommand";

        [Fact]
        public async Task BulkUpdateCategory_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var group = new ItemGroup() { Name = "Group 1", CompanyId = _actor.CompanyId };
            var group2 = new ItemGroup() { Name = "Group 2", CompanyId = _actor.CompanyId };
            var otherGroup = new ItemGroup() { Name = "Group 2", CompanyId = Guid.NewGuid() };
            dbContext.ItemGroups.AddRange(group, group2, otherGroup);

            var category1 = new ItemCategory() { Name = "Category 1", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category2 = new ItemCategory() { Name = "Category 2", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category3 = new ItemCategory() { Name = "Category 3", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category4 = new ItemCategory() { Name = "Other Category", GroupId = otherGroup.Id, CompanyId = Guid.NewGuid() };
            dbContext.ItemCategories.AddRange(category1, category2, category3, category4);


            var bulkUpload = new ItemCategoryBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Update, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemCategoryBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = category1.Id.ToString(), Column2 = "Update 1", Column3 = "Group 1" };
            var uploadDetail2 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = category2.Id.ToString(), Column2 = "Update 2", Column3 = "Group 2" };
            var uploadDetail3 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = category3.Id.ToString(), Column2 = "Update 3", Column3 = "Group 1" };

            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail3);

            await dbContext.SaveAsync(_actor);

            var service = new BulkUpdateCategoryService(dbContext, _actor, _langService);
            var mapping = new Dictionary<string, string>()
            {
                {"Id", "Column1" },
                {"Name", "Column2" },
                {"Group", "Column3" },
            };

            var result = await service.Save(mapping, cts.Token);
            var result_category1 = dbContext.ItemCategories.FirstOrDefault(x => x.Id == category1.Id);
            var result_category2 = dbContext.ItemCategories.FirstOrDefault(x => x.Id == category2.Id);
            var result_category3 = dbContext.ItemCategories.FirstOrDefault(x => x.Id == category3.Id);

            Assert.True(result);
            Assert.Equal("Update 1", result_category1!.Name);
            Assert.Equal("Update 2", result_category2!.Name);
            Assert.Equal("Update 3", result_category3!.Name);
            Assert.Equal(group2.Id, result_category2!.GroupId);
            Assert.Equal("Other Category", category4.Name);

            cts.Dispose();
        }


        [Fact]
        public async Task BulkUpdateCategory_Failed()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var group = new ItemGroup() { Name = "Group 1", CompanyId = _actor.CompanyId };
            var group2 = new ItemGroup() { Name = "Group 2", CompanyId = _actor.CompanyId };
            var otherGroup = new ItemGroup() { Name = "Group 2", CompanyId = Guid.NewGuid() };
            dbContext.ItemGroups.AddRange(group, group2, otherGroup);

            var category1 = new ItemCategory() { Name = "Category 1", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category2 = new ItemCategory() { Name = "Category 2", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category3 = new ItemCategory() { Name = "Category 3", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category4 = new ItemCategory() { Name = "Other Category", GroupId = otherGroup.Id, CompanyId = Guid.NewGuid() };
            dbContext.ItemCategories.AddRange(category1, category2, category3, category4);


            var bulkUpload = new ItemCategoryBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Update, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemCategoryBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = null, Column2 = "Update 1", Column3 = "Group 1" };
            var uploadDetail2 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = Guid.NewGuid().ToString(), Column2 = "Update 2", Column3 = "Group 2" };
            var uploadDetail3 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = "xxx", Column2 = "Update 3", Column3 = "Group 1" };
            var uploadDetail4 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = category3.Id.ToString(), Column2 = "Update 3", Column3 = "Group 3" };

            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail3);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail4);

            await dbContext.SaveAsync(_actor);

            var service = new BulkUpdateCategoryService(dbContext, _actor, _langService);
            var mapping = new Dictionary<string, string>()
            {
                {"Id", "Column1" },
                {"Name", "Column2" },
                {"Group", "Column3" },
            };
            var fieldId = _langService[$"{resources}.Id.ColumnHeader"];
            var result = await service.Save(mapping, cts.Token);
            var error1 = _langService.Format($"{resources}.KeyColumnIsEmpty.Message", bulkUpload.headerColum1);
            var error2 = _langService.Format($"{resources}.ValueNotFoundInTheSystem.Message", uploadDetail2.Column1);
            var error3 = string.Format(_langService[$"{resources}.CanotCoverValueAsField.Message"], uploadDetail3.Column1, fieldId);
            var error4 = _langService.Format($"{resources}.FieldNotExistInTheSystem.Message", uploadDetail4.Column3);

            Assert.False(result);
            Assert.Contains(error1, uploadDetail1.Errors);
            Assert.Contains(error2, uploadDetail2.Errors);
            Assert.Contains(error3, uploadDetail3.Errors);
            Assert.Contains(error4, uploadDetail4.Errors);

            cts.Dispose();
        }


    }
}
