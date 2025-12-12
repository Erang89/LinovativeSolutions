using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group
{
    public class BulkUpdateGroupService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationGroupBase(dbContext, actor, lang, CrudOperations.Update)
    {
        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var rows = GetRecords();
            var ids = GetInputValues(_columnId).Select(x => (Guid)x).ToList();
            var groups = _dbContext.ItemGroups.GetAll(_actor).Where(x => ids.Contains(x.Id)).ToList();
            var (cell, converter) = GetGetterAndConverter(_columnId);
            foreach (var row in rows)
            {
                Mappings(row, groups.FirstOrDefault(x => x.Id == (Guid)converter(cell(row))!));
            }

            await Task.CompletedTask;
        }

        private void Mappings(ItemGroupBulkUploadDetail row, ItemGroup? itemGroup)
        {
            if (itemGroup is null) return;

            foreach(var key in _fieldMapping.Keys)
            {
                Action<ItemGroupBulkUploadDetail, ItemGroup>? mapper = key switch
                {
                    Keys.Name => MapingName,
                    _ => null
                };

                if (mapper is not null) mapper(row, itemGroup);
            }
        }

        private void MapingName(ItemGroupBulkUploadDetail detail, ItemGroup group)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Name);
            group.Name = (string?)converter(cell(detail));
        }
    }
}
