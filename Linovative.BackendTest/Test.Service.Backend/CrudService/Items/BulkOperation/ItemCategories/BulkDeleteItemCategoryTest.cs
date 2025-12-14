using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Category;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using Microsoft.EntityFrameworkCore;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.BulkOperation.ItemCategories
{
    public class BulkDeleteItemCategoryTest : UseDatabaseTestBase
    {
        const string resources = "BulkUploadCommand";

        [Fact]
        public async Task DeleteCategory_Success()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var group = new ItemGroup() { Id = new Guid("52184401-8e8a-4687-9b1d-16af87900c1e"), Name = "Group 1", CompanyId = _actor.CompanyId };
            var otherGroup = new ItemGroup() { Name = "Group 1", CompanyId = Guid.NewGuid() };
            dbContext.ItemGroups.AddRange(group, otherGroup);

            var category1 = new ItemCategory() { Name = "Category 1", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category2 = new ItemCategory() { Name = "Category 2", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category4 = new ItemCategory() { Name = "Category 2", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category3 = new ItemCategory() { Name = "Category 3", GroupId = otherGroup.Id, CompanyId = Guid.NewGuid() };
            dbContext.ItemCategories.AddRange(category1, category2, category3, category4);


            var bulkUpload = new ItemCategoryBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Delete, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemCategoryBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = category1.Id.ToString(), Column2 = "xx", Column3 = "Group 1" };
            var uploadDetail2 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = category2.Id.ToString(), Column2 = "Category 2", Column3 = "Group 1" };
            var uploadDetail3 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = category4.Id.ToString(), Column2 = null, Column3 = "Group 1" };

            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail3);

            await dbContext.SaveAsync(_actor);

            var service = new BulkDeleteItemGroupService(dbContext, _actor, _langService);
            var mapping = new Dictionary<string, string>()
            {
                {"Id", "Column1" },
                {"Name", "Column2" },
            };

            var result = await service.Save(mapping, cts.Token);
            var newCategory1 = await dbContext.ItemCategories.FirstOrDefaultAsync(x => x.Id == new Guid(uploadDetail1.Column1)!);
            var newCategory2 = await dbContext.ItemCategories.FirstOrDefaultAsync(x => x.Id == new Guid(uploadDetail2.Column1!));
            var newCategory4 = await dbContext.ItemCategories.FirstOrDefaultAsync(x => x.Id == new Guid(uploadDetail3.Column1!));
            var otherCategory = await dbContext.ItemCategories.FirstOrDefaultAsync(x => x.Id == category3.Id);

            Assert.True(result);
            Assert.NotNull(newCategory1);
            Assert.NotNull(newCategory2);
            Assert.NotNull(otherCategory);
            Assert.True(newCategory1.IsDeleted);
            Assert.True(newCategory2.IsDeleted);
            Assert.True(newCategory4!.IsDeleted);
            Assert.Equal(newCategory1.LastModifiedBy, _actor.UserId);
            Assert.Equal(newCategory2.LastModifiedBy, _actor.UserId);

            cts.Dispose();
        }


        [Fact]
        public async Task DeleteCategory_Failed()
        {
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();

            var group = new ItemGroup() { Id = new Guid("52184401-8e8a-4687-9b1d-16af87900c1e"), Name = "Group 1", CompanyId = _actor.CompanyId };
            var otherGroup = new ItemGroup() { Name = "Group 1", CompanyId = Guid.NewGuid() };
            dbContext.ItemGroups.AddRange(group, otherGroup);

            var category1 = new ItemCategory() { Name = "Category 1", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category2 = new ItemCategory() { Name = "Category 2", GroupId = group.Id, CompanyId = _actor.CompanyId };
            var category3 = new ItemCategory() { Name = "Category 3", GroupId = otherGroup.Id, CompanyId = Guid.NewGuid() };
            dbContext.ItemCategories.AddRange(category1, category2, category3);


            var bulkUpload = new ItemCategoryBulkUpload() { Id = Guid.NewGuid(), headerColum1 = "Id", headerColum2 = "Name", Operation = CrudOperations.Delete, UserId = _actor.UserId, CompanyId = _actor.CompanyId!.Value };
            dbContext.ItemCategoryBulkUploads.Add(bulkUpload);

            var uploadDetail1 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = null, Column2 = "Create Category 1", Column3 = "Group 1" };
            var uploadDetail2 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = category3.Id.ToString(), Column2 = "Create Category 2", Column3 = "Group 1" };
            var uploadDetail3 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = "", Column2 = "Create Category 3", Column3 = "Group 1" };
            var uploadDetail4 = new ItemCategoryBulkUploadDetail() { ItemCategoryBulkUploadId = bulkUpload.Id, Column1 = "xxx", Column2 = "Category 3", Column3 = "Group 1" };

            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail1);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail2);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail3);
            dbContext.ItemCategoryBulkUploadDetails.Add(uploadDetail4);

            await dbContext.SaveAsync(_actor);

            var service = new BulkDeleteItemGroupService(dbContext, _actor, _langService);
            var mapping = new Dictionary<string, string>()
            {
                {"Id", "Column1" },
                {"Name", "Column2" },
            };

            var result = await service.Save(mapping, cts.Token);
            var error1 = _langService.Format($"{resources}.KeyColumnIsEmpty.Message", bulkUpload.headerColum1);            
            var error2 = _langService.Format($"{resources}.ValueNotFoundInTheSystem.Message", uploadDetail2.Column1);
            var error3 = _langService.Format($"{resources}.KeyColumnIsEmpty.Message", bulkUpload.headerColum1);
            var idFieldName = _langService[$"{resources}.Id.ColumnHeader"];
            var error4 = string.Format(_langService[$"{resources}.CanotCoverValueAsField.Message"], uploadDetail4.Column1, idFieldName);

            Assert.False(result);
            Assert.Contains(error1, uploadDetail1.Errors);
            Assert.Contains(error2, uploadDetail2.Errors);
            Assert.Contains(error3, uploadDetail3.Errors);
            Assert.Contains(error4, uploadDetail4.Errors);

            cts.Dispose();
        }
    }
}
