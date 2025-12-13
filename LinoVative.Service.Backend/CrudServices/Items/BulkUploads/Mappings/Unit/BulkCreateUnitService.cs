using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Unit
{
    public class BulkCreateUnitService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationnitBase(dbContext, actor, lang, CrudOperations.Create)
    {
        protected override List<string> RequieredFieldWhenCreated => [Keys.Name];

        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var rows = GetRecords();
            foreach (var row in rows) MapRow(row);
            await Task.CompletedTask;
        }


        private void MapRow(ItemUnitBulkUploadDetail row)
        {
            var unit = new ItemUnit() { CompanyId = _actor.CompanyId};
            foreach(var key in _fieldMapping.Keys)
            {
                Action<ItemUnitBulkUploadDetail, ItemUnit>? mapper = key switch
                {
                    Keys.Id => MappingId,
                    Keys.Name => MappingName,
                    _ => null
                };
                
                if (mapper == null) continue;

                mapper(row, unit);
            }

            unit.CreateBy(_actor);
            _dbContext.ItemUnits.Add(unit);
        }



        private void MappingName(ItemUnitBulkUploadDetail row, ItemUnit unit)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Name);
            unit.Name = (string)converter(cell(row)!)!;
        }



        private void MappingId(ItemUnitBulkUploadDetail row, ItemUnit unit)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Id);
            if (string.IsNullOrWhiteSpace(cell(row))) return;
            unit.Id = (Guid)converter(cell(row)!)!;
        }
    }
}
