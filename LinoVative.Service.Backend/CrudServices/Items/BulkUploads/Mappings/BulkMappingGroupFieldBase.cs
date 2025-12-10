using LinoVative.Shared.Dto.BulkUploads;
using LinoVative.Shared.Dto.ItemDtos;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings
{
    public abstract class BulkMappingGroupFieldBase
    {
        protected static class Fields
        {
            public const string Id = nameof(ItemGroupViewDto.Id);
            public const string Name = nameof(ItemGroupViewDto.Name);
        }

        protected static class ExcelColumns
        {
            public const string Column1 = nameof(BulkUploadItemGroupDetailDto.Column1);
            public const string Column2 = nameof(BulkUploadItemGroupDetailDto.Column2);
        }
    }


}
