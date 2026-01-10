using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.Items.Helpers;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Item
{
    public class BulkUpdateItemService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationItemBase(dbContext, actor, lang, CrudOperations.Update)
    {
        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var rows = GetRecords();
            var ids = GetInputValues(_columnId).Select(x => (Guid)x!).ToList();
            var items = await _dbContext.SKUItems.GetAll(_actor).Where(x => ids.Contains(x.Id)).Include(x => x.Item).ToListAsync(token);
            var (cell, converter) = GetGetterAndConverter(_columnId);
            foreach (var row in rows)
            {
                Mappings(row, items.FirstOrDefault(x => x.Id == (Guid)converter(cell(row))!));
            }

            await Task.CompletedTask;
        }

        private void Mappings(ItemBulkUploadDetail row, SKUItem? item)
        {
            if (item is null) return;

            foreach(var key in _fieldMapping.Keys)
            {
                Action<ItemBulkUploadDetail, SKUItem>? mapper = key switch
                {
                    Keys.ItemName => MapingName,
                    Keys.CategoryName => MappingCategory,
                    Keys.SKU => MappingCode,
                    Keys.VarianName => MappingVariantName,
                    Keys.UnitName => MappingUnit,
                    Keys.IsActive => MappingActive,
                    Keys.IsPurchaseItem => MappingIsPurchaseItem,
                    Keys.IsSaleItem => MappingIsSaleItem,
                    Keys.SalePrice => MappingSellPrice,
                    Keys.DefaultPurchaseQty => MappingDefaultPurchaseQty,
                    Keys.MinimumStockQty => MappingMinimumStockQty,
                    Keys.ItemDescription => MappingDescription,
                    _ => null
                };

                if (mapper is not null) mapper(row, item);
            }

            if (_dbContext.GetEntityState(item) == EntityState.Modified)
                item.ModifyBy(_actor);

        }

        private void MappingCode(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.SKU);
            item.SKU = (string)converter(cell(detail)!)!;
        }

        private void MappingSellPrice(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.SalePrice);
            item.SalePrice = (decimal)converter(cell(detail)!)!;
        }

        private void MappingDefaultPurchaseQty(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.DefaultPurchaseQty);
            item.DefaultPurchaseQty = (decimal)converter(cell(detail)!)!;
        }

        private void MappingMinimumStockQty(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.MinimumStockQty);
            item.MinimumStockQty = (decimal)converter(cell(detail)!)!;
        }

        private void MappingActive(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.IsActive);

            if (string.IsNullOrWhiteSpace(cell(detail))) return;

            item.IsActive = (bool)converter(cell(detail)!)!;
        }

        private void MappingIsPurchaseItem(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.DefaultPurchaseQty);

            if (string.IsNullOrWhiteSpace(cell(detail))) return;

            item.Item.CanBePurchased= (bool)converter(cell(detail)!)!;
        }

        private void MappingIsSaleItem(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.IsSaleItem);

            if (string.IsNullOrWhiteSpace(cell(detail))) return;

            item.Item.CanBeSell = (bool)converter(cell(detail)!)!;
        }

        List<ItemCategory>? _categories = null;
        private void MappingCategory(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, _) = GetGetterAndConverter(Keys.CategoryName);
            var inputValues = GetInputValues(Keys.CategoryName).Select(x => (string)x!).ToList();

            _categories ??= [.. _dbContext.ItemCategories.GetAll(_actor).Where(x => inputValues.Contains(x.Name!))];

            var category = _categories.FirstOrDefault(x => x.Name!.Equals(cell(detail), StringComparison.CurrentCultureIgnoreCase));
            if (category == null) return;

            item.Item.CategoryId = category.Id;
        }

        List<ItemUnit>? _units = null;
        private void MappingUnit(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, _) = GetGetterAndConverter(Keys.UnitName);
            var inputValues = GetInputValues(Keys.UnitName).Select(x => (string)x!).ToList();

            _units ??= [.. _dbContext.ItemUnits.GetAll(_actor).Where(x => inputValues.Contains(x.Name!))];

            var unit = _units.FirstOrDefault(x => x.Name!.Equals(cell(detail), StringComparison.CurrentCultureIgnoreCase));
            if (unit == null) return;

            item.UnitId = unit.Id;
        }

        private void MappingVariantName(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.VarianName);
            item.VarianName = (string)converter(cell(detail)!)!;
        }


        private void MappingDescription(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.ItemDescription);
            if (string.IsNullOrWhiteSpace(cell(detail))) return;
            item.Item.Notes = (string)converter(cell(detail)!)!;
        }


        private void MapingName(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.ItemName);
            item.Item.Name = (string?)converter(cell(detail));
            item.Item.UpdateItemNameInOtherTable(_dbContext);
        }
    }
}
