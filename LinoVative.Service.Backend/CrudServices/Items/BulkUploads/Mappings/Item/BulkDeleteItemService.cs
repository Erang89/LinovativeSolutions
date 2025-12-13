using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Item
{
    public class BulkDeleteItemService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationItemBase(dbContext, actor, lang, CrudOperations.Delete)
    {
        protected override async Task BulkOperationHandler(CancellationToken token)
        {   
            var inputIds = GetInputValues(_columnId).Select(x => (Guid)x!).ToList();
            var items = _dbContext.Items.GetAll(_actor).Where(x => inputIds.Contains(x.Id)).ToList();
            foreach (var item in items)
            {
                item.Delete(_actor);
            }

            await Task.CompletedTask;
        }
    }
}
