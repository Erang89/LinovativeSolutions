using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Unit
{
    public class BulkDeleteUnitService(IAppDbContext dbContext, IActor actor, ILangueageService lang) : BulkOperationnitBase(dbContext, actor, lang, CrudOperations.Delete)
    {
        protected override async Task BulkOperationHandler(CancellationToken token)
        {   
            var inputIds = GetInputValues(_columnId).Select(x => (Guid)x!).ToList();
            var units = _dbContext.ItemUnits.GetAll(_actor).Where(x => inputIds.Contains(x.Id)).ToList();
            foreach (var group in units)
            {
                group.Delete(_actor);
            }

            await Task.CompletedTask;
        }
    }
}
