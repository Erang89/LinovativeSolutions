using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Category
{
    public class BulkDeleteItemGroupService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationCategoryBase(dbContext, actor, lang, CrudOperations.Delete)
    {
        protected override async Task BulkOperationHandler(CancellationToken token)
        {
            var inputIds = GetInputValues(_columnId).Select(x => (Guid)x!).ToList();
            var categories = _dbContext.ItemCategories.GetAll(_actor).Where(x => inputIds.Contains(x.Id)).ToList();

            foreach (var group in categories)
            {
                group.Delete(_actor);
            }

            await Task.CompletedTask;
        }
    }
}
