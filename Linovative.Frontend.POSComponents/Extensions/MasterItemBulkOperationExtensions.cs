using Linovative.Frontend.POSComponents.Enums;
using Linovative.Shared.Interface.Enums;

namespace Linovative.Frontend.POSComponents.Extensions
{
    internal static class MasterItemBulkOperationExtensions
    {
        public static CrudOperations ToCrudOperations(this BulkOperations? SelectedOperation, BulkActionTypes? SelectedActionTypes)
        {
            if (SelectedActionTypes == BulkActionTypes.Update_DownloadFromDataMapping)
                return CrudOperations.Mapping;

            if (SelectedActionTypes == BulkActionTypes.Delete_DownloadFromDataMapping)
                return CrudOperations.MappingForDelete;

            return SelectedOperation switch
            {
                BulkOperations.Create => CrudOperations.Create,
                BulkOperations.Update => CrudOperations.Update,
                BulkOperations.Delete => CrudOperations.Delete,
                _ => CrudOperations.Create,
            };
        }
    }
}
