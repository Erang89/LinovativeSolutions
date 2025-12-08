using ClosedXML.Excel;
using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Commons;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download
{
    public class DownloadItemCategoryCommand : IRequest<Result>
    {
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class DownloadItemCategoryHandler : QueryServiceBase<ItemCategory, DownloadItemCategoryCommand>, IRequestHandler<DownloadItemCategoryCommand, Result>
    {
        private readonly ILangueageService _lang;
        public DownloadItemCategoryHandler(ILangueageService lang, IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) :
            base(dbContext, actor, mapper, appCache)
        {
            _lang = lang;
            _lang.EnsureLoad(BulkUploadSettings.BulkUploadCommand);
        }


        public async Task<Result> Handle(DownloadItemCategoryCommand request, CancellationToken ct)
        {
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ExcelFiles", "UpdateCategoryTemplate.xlsx");
            var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet(1);
            var sheetName = _lang[$"{BulkUploadSettings.BulkUploadCommand}.UpdateCategory.SheetName"];
            ws.Name = sheetName;


            ws.Cells("A1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.Id.ColumnHeader"];
            ws.Cells("B1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.ItemGroupName.ColumnHeader"];
            ws.Cells("C1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.ItemCategoryName.ColumnHeader"];

            ws.Cells("A1:C1").Style.Font.Bold = true;

            ws.Column("A").Width = 45;
            ws.Column("B").Width = 70;
            ws.Column("C").Width = 70;
            
            var rowNumber = 2;
            var categories = base.GetAll().ApplyFilters(request.Filter).Select(x => new { x.Id, x.Name, GroupName = x.Group!.Name }).ToList();

            foreach (var g in categories)
            {
                var row = ws.Row(rowNumber);
                row.Cell("A").Value = g.Id.ToString();
                row.Cell("B").Value = g.GroupName?.ToString();
                row.Cell("C").Value = g.Name?.ToString();
                rowNumber++;
            }

            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return Result.OK(ms);
        }
    }
}
