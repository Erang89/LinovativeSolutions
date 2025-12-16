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
        protected override List<string> RequieredFieldWhenCreated => [Keys.Name, Keys.Code, Keys.Unit, Keys.Category, Keys.SellPrice];

        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var rows = GetRecords();
            foreach (var row in rows) MapRow(row);
            await Task.CompletedTask;
        }


        private void MapRow(ItemBulkUploadDetail row)
        {
            var unit = new ItemMaster() { CompanyId = _actor.CompanyId, IsActive = true};
            foreach(var key in _fieldMapping.Keys)
            {
                Action<ItemBulkUploadDetail , ItemMaster>? mapper = key switch
                {
                    Keys.Id => MappingId,
                    Keys.Name => MappingName,
                    Keys.Description => MappingDescription,
                    Keys.Unit => MappingUnit,
                    Keys.Category => MappingCategory,
                    Keys.IsActive => MappingActive,
                    Keys.SellPrice => MappingSellPrice,
                    Keys.Code => MappingCode,
                    _ => null
                };
                
                if (mapper == null) continue;

                mapper(row, unit);
            }

            unit.CreateBy(_actor);
            _dbContext.Items.Add(unit);
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

            item.UnitId = unit.Id;
        }

        private void MappingDescription(ItemBulkUploadDetail detail, ItemMaster item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Description);
            item.Description = (string)converter(cell(detail)!)!;
        }

        private void MappingName(ItemBulkUploadDetail  row, ItemMaster item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Name);
            item.Name = (string)converter(cell(row)!)!;
        }

        private void MappingId(ItemBulkUploadDetail  row, ItemMaster item)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Id);
            if (string.IsNullOrWhiteSpace(cell(row))) return;
            item.Id = (Guid)converter(cell(row)!)!;
        }
    }
}
