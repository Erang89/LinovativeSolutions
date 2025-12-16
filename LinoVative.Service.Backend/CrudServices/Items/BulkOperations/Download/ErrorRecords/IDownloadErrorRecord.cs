namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperations.Download.ErrorRecords
{
    public interface IDownloadErrorRecord
    {
        public Task<MemoryStream> DownloadRows();
    }
}
