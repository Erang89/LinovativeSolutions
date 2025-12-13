using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Unit
{
    public class BulkUpdateUnitService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationnitBase(dbContext, actor, lang, CrudOperations.Update)
    {
        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var rows = GetRecords();
            var ids = GetInputValues(_columnId).Select(x => (Guid)x!).ToList();
            var units = await _dbContext.ItemUnits.GetAll(_actor).Where(x => ids.Contains(x.Id)).ToListAsync(token);
            var (cell, converter) = GetGetterAndConverter(_columnId);
            foreach (var row in rows)
            {
                Mappings(row, units.FirstOrDefault(x => x.Id == (Guid)converter(cell(row))!));
            }

            await Task.CompletedTask;
        }

        private void Mappings(ItemUnitBulkUploadDetail row, ItemUnit? unit)
        {
            if (unit is null) return;

            foreach(var key in _fieldMapping.Keys)
            {
                Action<ItemUnitBulkUploadDetail, ItemUnit>? mapper = key switch
                {
                    Keys.Name => MapingName,
                    _ => null
                };

                if (mapper is not null) mapper(row, unit);
            }

            if (_dbContext.GetEntityState(unit) == EntityState.Modified)
                unit.ModifyBy(_actor);

        }

        private void MapingName(ItemUnitBulkUploadDetail detail, ItemUnit unit)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Name);
            unit.Name = (string?)converter(cell(detail));
        }
    }
}
