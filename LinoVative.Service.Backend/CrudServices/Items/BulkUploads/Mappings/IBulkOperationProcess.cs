using LinoVative.Shared.Dto;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings
{

    public interface IBulkOperationProcess
    {
        Task<Result> Validate(Dictionary<string, string> fieldMapping, CancellationToken token);
        Task<Result> Save(Dictionary<string, string> fieldMapping, CancellationToken token);
    }
}
