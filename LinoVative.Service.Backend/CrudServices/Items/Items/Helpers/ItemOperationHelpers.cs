using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Items;

namespace LinoVative.Service.Backend.CrudServices.Items.Items.Helpers
{
    internal static class ItemOperationHelpers
    {
        public static void UpdateItemNameInOtherTable(this Item item, IAppDbContext dbContext)
        {
            item.UpdateOutletExceptionItems(dbContext);
        }

        private static void UpdateOutletExceptionItems(this Item item, IAppDbContext dbContext)
        {
            var itemExceptions = dbContext.OutletItemExceptionals.Where(x => x.EntityId == item.Id).ToList();
            foreach (var itemException in itemExceptions)
            {
                itemException.Name = item.Name;
            }
        }
    }
}
