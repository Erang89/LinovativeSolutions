using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using ItemMaster = LinoVative.Service.Core.Items.Item;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Item
{
    public class BulkCreateItemService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationItemBase(dbContext, actor, lang, CrudOperations.Create)
    {
        protected override List<string> RequieredFieldWhenCreated => [Keys.ItemName,  Keys.SKU, Keys.VarianName, Keys.UnitName, Keys.CategoryName];

        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var rows = GetRecords();
            foreach (var row in rows) MapRow(row);
            await Task.CompletedTask;
        }

        List<string> GetOrderKeys()
        {
            Func<string, int> keySeq = (key) => key switch
            {
                Keys.Id => 1,
                Keys.ItemName => 2,
                Keys.CategoryName => 3,
                Keys.SKU => 4,
                Keys.VarianName => 5,
                Keys.UnitName => 6,
                Keys.IsActive => 7,
                Keys.IsPurchaseItem => 8,
                Keys.IsSaleItem => 9,
                Keys.SalePrice => 10,
                Keys.DefaultPurchaseQty => 11,
                Keys.MinimumStockQty => 12,
                Keys.ItemDescription => 13,
                _ => 100
            };

            return _fieldMapping.Keys.OrderBy(x => keySeq(x)).ToList();
        }

        private void MapRow(ItemBulkUploadDetail row)
        {
            var item = new SKUItem() { CompanyId = _actor.CompanyId, IsActive = true};
            
            foreach(var key in GetOrderKeys())
            {
                Action<ItemBulkUploadDetail , SKUItem>? mapper = key switch
                {
                    Keys.Id => MappingId,
                    Keys.ItemName => MappingName,
                    Keys.CategoryName => MappingCategory,
                    Keys.SKU => MappingCode,
                    Keys.VarianName => MappingVariantName,
                    Keys.UnitName => MappingUnit,
                    Keys.IsActive => MappingActive,
                    Keys.IsPurchaseItem => MappingIsPurchaseItem,
                    Keys.IsSaleItem => MappingIsSalesItem,
                    Keys.SalePrice => MappingSellPrice,
                    Keys.DefaultPurchaseQty => MappingDefaultPurchaseQty,
                    Keys.MinimumStockQty => MappingMinimumStockQty,
                    Keys.ItemDescription => MappingDescription,
                    _ => null
                };
                
                if (mapper == null) continue;
                mapper(row, item);
            }

            item.CreateBy(_actor);
            _dbContext.SKUItems.Add(item);
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
            var (cell, converter) = GetGetterAndConverter(Keys.IsPurchaseItem);

            if (string.IsNullOrWhiteSpace(cell(detail)) || item.Item is null) return;

            item.Item.CanBePurchased = (bool)converter(cell(detail)!)!;
        }


        private void MappingIsSalesItem(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.IsSaleItem);

            if (string.IsNullOrWhiteSpace(cell(detail)) || item.Item is null) return;

            item.Item.CanBeSell = (bool)converter(cell(detail)!)!;
        }

        List<ItemCategory>? _categories = null;
        private void MappingCategory(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, _) = GetGetterAndConverter(Keys.CategoryName);
            var inputValues = GetInputValues(Keys.CategoryName).Select(x => (string)x!).ToList();

            _categories ??= [.. _dbContext.ItemCategories.GetAll(_actor).Where(x => inputValues.Contains(x.Name!))];

            var category = _categories.FirstOrDefault(x => x.Name!.Equals(cell(detail), StringComparison.CurrentCultureIgnoreCase));
            if (category == null || item.Item is null) return;

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

        private void MappingDescription(ItemBulkUploadDetail detail, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.ItemDescription);
            if (item.Item is null || string.IsNullOrWhiteSpace(cell(detail))) return;

            item.Item.Notes = (string)converter(cell(detail)!)!;
        }

        List<ItemMaster>? items { get; set; }
        List<ItemMaster> GetItems()
        {
            if(items is not null) return items;

            var itemNames = GetInputValues(Keys.ItemName).Select(x => (string)x!).ToList();
            items = _dbContext.Items.GetAll(_actor).Where(x => itemNames.Contains(x.Name!)).ToList();
            items ??= new();
            return items;
        }



        ItemMaster? GetItemByName(string itemName) => GetItems().FirstOrDefault(x => x.Name!.ToLower().Contains(itemName.ToLower()));
        
        private void MappingName(ItemBulkUploadDetail  row, SKUItem item)
        {
            var (cell, _) = GetGetterAndConverter(Keys.ItemName);

            if(item.Item is not null)
            {
                item.Item.Name = cell(row);
                return;
            }
            

            var itemMaster = GetItemByName(cell(row)!);
            if(itemMaster is null)
            {
                itemMaster = new ItemMaster() { CompanyId = _actor.CompanyId, Name = cell(row)! };
                items!.Add(itemMaster);
                _dbContext.Items.Add(itemMaster);
                item.ItemId = itemMaster.Id;
                item.Item = itemMaster;
                return;
            }

            item.Item = itemMaster;
            item.ItemId = itemMaster.Id;
        }


        private void MappingVariantName(ItemBulkUploadDetail  row, SKUItem item)
        {
            var (cell, _) = GetGetterAndConverter(Keys.VarianName);
            item.VarianName = cell(row)!;
        }


        private void MappingId(ItemBulkUploadDetail  row, SKUItem item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Id);
            if (string.IsNullOrWhiteSpace(cell(row))) return;
            item.Id = (Guid)converter(cell(row)!)!;
        }
    }
}
