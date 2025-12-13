using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Category
{
    public class BulkUpdateCategoryService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationCategoryBase(dbContext, actor, lang, CrudOperations.Create)
    {
        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var rows = GetRecords();
            var ids = GetInputValues(_columnId).Select(x => (Guid)x!).ToList();
            var categories = await _dbContext.ItemCategories.GetAll(_actor).Where(x => ids.Contains(x.Id)).ToListAsync(token);
            var (cell, converter) = GetGetterAndConverter(Keys.Id);

            foreach (var row in rows)
            {
                MapRow(row, categories.FirstOrDefault(x => x.Id == (Guid)converter(cell(row))!));
            }

            await Task.CompletedTask;
        }


        private void MapRow(ItemCategoryBulkUploadDetail row, ItemCategory? category)
        {
            if (category is null) return;

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
