using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ItemMaster = LinoVative.Service.Core.Items.Item;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Item
{
    public class BulkUpdateItemService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationItemBase(dbContext, actor, lang, CrudOperations.Update)
    {
        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var rows = GetRecords();
            var ids = GetInputValues(_columnId).Select(x => (Guid)x!).ToList();
            var items = await _dbContext.Items.GetAll(_actor).Where(x => ids.Contains(x.Id)).ToListAsync(token);
            var (cell, converter) = GetGetterAndConverter(_columnId);
            foreach (var row in rows)
            {
                Mappings(row, items.FirstOrDefault(x => x.Id == (Guid)converter(cell(row))!));
            }

            await Task.CompletedTask;
        }

        private void Mappings(ItemBulkUploadDetail row, ItemMaster? unit)
        {
            if (unit is null) return;

            foreach(var key in _fieldMapping.Keys)
            {
                Action<ItemBulkUploadDetail, ItemMaster>? mapper = key switch
                {
                    Keys.Name => MapingName,
                    Keys.Code => MappingCode,
                    Keys.Description => MappingDescription,
                    Keys.Unit => MappingUnit,
                    Keys.Category => MappingCategory,
                    Keys.IsActive => MappingActive,
                    Keys.SellPrice => MappingSellPrice,
                    _ => null
                };

                if (mapper is not null) mapper(row, unit);
            }

            if (_dbContext.GetEntityState(unit) == EntityState.Modified)
                unit.ModifyBy(_actor);

        }

        private void MappingCode(ItemBulkUploadDetail detail, ItemMaster item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Code);
            item.Code = (string)converter(cell(detail)!)!;
        }

        private void MappingSellPrice(ItemBulkUploadDetail detail, ItemMaster item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.SellPrice);
            item.SellPrice = (decimal)converter(cell(detail)!)!;
        }

        private void MappingActive(ItemBulkUploadDetail detail, ItemMaster item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.IsActive);

            if (string.IsNullOrWhiteSpace(cell(detail))) return;

            item.IsActive = (bool)converter(cell(detail)!)!;
        }

        List<ItemCategory>? _categories = null;
        private void MappingCategory(ItemBulkUploadDetail detail, ItemMaster item)
        {
            var (cell, _) = GetGetterAndConverter(Keys.Category);
            var inputValues = GetInputValues(Keys.Category).Select(x => (string)x!).ToList();

            _categories ??= [.. _dbContext.ItemCategories.GetAll(_actor).Where(x => inputValues.Contains(x.Name!))];

            var category = _categories.FirstOrDefault(x => x.Name!.Equals(cell(detail), StringComparison.CurrentCultureIgnoreCase));
            if (category == null) return;

            item.CategoryId = category.Id;
        }

        List<ItemUnit>? _units = null;
        private void MappingUnit(ItemBulkUploadDetail detail, ItemMaster item)
        {
            var (cell, _) = GetGetterAndConverter(Keys.Unit);
            var inputValues = GetInputValues(Keys.Unit).Select(x => (string)x!).ToList();

            _units ??= [.. _dbContext.ItemUnits.GetAll(_actor).Where(x => inputValues.Contains(x.Name!))];

            var unit = _units.FirstOrDefault(x => x.Name!.Equals(cell(detail), StringComparison.CurrentCultureIgnoreCase));
            if (unit == null) return;

            item.CategoryId = unit.Id;
        }

        private void MappingDescription(ItemBulkUploadDetail detail, ItemMaster item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Description);
            item.Description = (string)converter(cell(detail)!)!;
        }

        private void MapingName(ItemBulkUploadDetail detail, ItemMaster item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Name);
            item.Name = (string?)converter(cell(detail));
        }
    }
}
