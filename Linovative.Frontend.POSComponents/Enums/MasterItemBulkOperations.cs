namespace Linovative.Frontend.POSComponents.Enums
{
    public enum BulkDataEntityTypes
    {
        ItemMaster,
        Group,
        Category,
        Unit
    }

    public enum BulkOperations
    {
        Create,
        Update,
        Delete
    }

    public enum BulkActionTypes
    {
        Create_DownloadTemplate,
        Create_Upload,
        Update_DownloadBlankTemplate,
        Update_DownloadFromData,
        Update_DownloadFromDataMapping,
        Update_Upload,
        Delete_DownloadBlankTemplate,
        Delete_DownloadFromDataFiltering,
        Delete_DownloadFromDataMapping,
        Delete_Upload
    }
}
