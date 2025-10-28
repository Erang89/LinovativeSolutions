using LinoVative.Shared.Dto.Interfaces;

namespace LinoVative.Shared.Dto.Commons
{
    public class BulkDeleteDto : IBulkDeleteDto
    {
        public List<Guid> Ids { get; set; } = new();
    }
}
