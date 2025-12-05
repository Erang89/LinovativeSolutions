namespace LinoVative.Shared.Dto.BulkUploads
{
    public class BulkUploadItemGroupDto : EntityDtoBase
    {
        public Guid? UserId { get; set; }
        public string? headerColum1 { get; set; }
        public List<BulkUploadItemGroupDetailDto> Details { get; set; } = new();
    }

    public class BulkUploadItemGroupDetailDto : EntityDtoBase
    {
        public Guid? ItemGroupBulkUploadId { get; set; }

        public string? Column1 { get; set; }
    }
}
