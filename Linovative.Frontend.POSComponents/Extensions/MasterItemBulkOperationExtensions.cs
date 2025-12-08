using Linovative.Frontend.POSComponents.Enums;
using Linovative.Shared.Interface.Enums;

namespace Linovative.Frontend.POSComponents.Extensions
{
    internal static class MasterItemBulkOperationExtensions
    {
        public static CrudOperations ToCrudOperations(this BulkOperations? SelectedOperation, BulkActionTypes? SelectedActionTypes)
        {
            CrudOperations operation = (SelectedActionTypes is BulkActionTypes.Update_DownloadFromDataMapping or BulkActionTypes.Delete_DownloadFromDataMapping) ?
            CrudOperations.Mapping :
            (SelectedOperation switch
            {
                BulkOperations.Create => CrudOperations.Create,
                BulkOperations.Update => CrudOperations.Update,
                BulkOperations.Delete => CrudOperations.Delete,
                _ => CrudOperations.Create,
            });

            return operation;
        }
    }
}
