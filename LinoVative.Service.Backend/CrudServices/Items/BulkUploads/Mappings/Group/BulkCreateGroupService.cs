using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group
{
    public class BulkCreateGroupService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationGroupBase(dbContext, actor, lang, CrudOperations.Create)
    {
        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var rows = GetRecords();
            foreach (var row in rows) MapRow(row);
            await Task.CompletedTask;
        }


        private void MapRow(ItemGroupBulkUploadDetail row)
        {
            var group = new ItemGroup() { CompanyId = _actor.CompanyId};
            foreach(var key in _fieldMapping.Keys)
            {
                Action<ItemGroupBulkUploadDetail, ItemGroup>? mapper = key switch
                {
                    Keys.Id => MappingId,
                    Keys.Name => MappingName,
                    _ => null
                };
                if(mapper != null) mapper(row, group);
            }

            group.CreateBy(_actor);
            _dbContext.ItemGroups.Add(group);
        }



        private void MappingName(ItemGroupBulkUploadDetail row, ItemGroup group)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Name);
            group.Name = (string)converter(cell(row)!)!;
        }



        private void MappingId(ItemGroupBulkUploadDetail row, ItemGroup group)
        {
            var (cell, converter) = GetGetterAndConverter(Keys.Id);
            if (string.IsNullOrWhiteSpace(cell(row))) return;
            group.Id = (Guid)converter(cell(row)!)!;
        }
    }
}
