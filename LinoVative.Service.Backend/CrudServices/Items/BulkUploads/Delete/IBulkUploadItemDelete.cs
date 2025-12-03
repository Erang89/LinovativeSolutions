namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Delete
{
    public interface IBulkUploadItemDelete
    {
        public Task Delete(CancellationToken token);
    }
}
