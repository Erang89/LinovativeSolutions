using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group
{
    public class BulkDeleteItemGroupService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationGroupBase(dbContext, actor, lang, CrudOperations.Delete)
    {
        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var groups = _dbContext.ItemGroups.GetAll(_actor);
            var inputIds = GetInputValues(_columnId).Select(x => (Guid)x!).ToList();
            foreach(var group in groups.Where(x => inputIds.Contains(x.Id)))
            {
                group.Delete(_actor);
            }

            await Task.CompletedTask;
        }
    }
}
