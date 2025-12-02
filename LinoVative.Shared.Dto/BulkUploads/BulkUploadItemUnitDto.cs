namespace LinoVative.Shared.Dto.BulkUploads
{
    public class BulkUploadItemUnitDto : EntityDtoBase
    {
        public Guid? UserId { get; set; }
        public string? headerColum1 { get; set; }
        public string? headerColum2 { get; set; }
        public List<BulkUploadItemUnitDetailDto> Details { get; set; } = new();
    }

    public class BulkUploadItemUnitDetailDto : EntityDtoBase
    {
        public Guid? ItemUnitBulkUploadId { get; set; }

        public string? Column1 { get; set; }
        public string? Column2 { get; set; }
    }
}
