using LinoVative.Shared.Dto;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings
{

    public interface IBulkMapping
    {
        Task<Result> Validate(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token);
        Task<Result> Save(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token);
    }
}
