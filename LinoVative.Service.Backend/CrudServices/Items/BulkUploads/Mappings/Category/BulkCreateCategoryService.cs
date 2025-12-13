using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Category
{
    public class BulkCreateCategoryService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationCategoryBase(dbContext, actor, lang, CrudOperations.Create)
    {

        protected override List<string> RequieredFieldWhenCreated => [Keys.Name, Keys.Group];

        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var rows = GetRecords();
            foreach (var row in rows) MapRow(row);
            await Task.CompletedTask;
        }


        private void MapRow(ItemCategoryBulkUploadDetail row)
        {
            var category = new ItemCategory() { CompanyId = _actor.CompanyId};
            foreach(var key in _fieldMapping.Keys)
            {
                Action<ItemCategoryBulkUploadDetail, ItemCategory>? mapper = key switch
                {
                    Keys.Id => MappingId,
                    Keys.Name => MappingName,
                    Keys.Group => MappingGroup,
                    _ => null
                };

                if (mapper == null) continue;

                mapper(row, category);
            }

            category.CreateBy(_actor);
            _dbContext.ItemCategories.Add(category);
        }

        List<ItemGroup>? _groups = null;
        private void MappingGroup(ItemCategoryBulkUploadDetail detail, ItemCategory category)
        {
            var inputValues = GetInputValues(Keys.Group).Select(x => (string)x!).ToList();
            var (cell, _) = GetGetterAndConverter(Keys.Group);

            _groups ??= [.. _dbContext.ItemGroups.GetAll(_actor).Where(x => inputValues.Contains(x.Name!))];

            var selectedGroup = _groups.FirstOrDefault(x => x.Name!.Equals(cell(detail), StringComparison.CurrentCultureIgnoreCase));
            if (selectedGroup is null) return;

            category.GroupId = selectedGroup.Id;
        }

        private void MappingName(ItemCategoryBulkUploadDetail row, ItemCategory category)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Name);
            category.Name = (string)converter(cell(row)!)!;
        }



        private void MappingId(ItemCategoryBulkUploadDetail row, ItemCategory category)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Id);
            if (string.IsNullOrWhiteSpace(cell(row))) return;
            category.Id = (Guid)converter(cell(row)!)!;
        }
    }
}
