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
    public class DownloadItemUnitCommand : IRequest<Result>
    {
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class DownloadItemUnitHandler : QueryServiceBase<ItemUnit, DownloadItemUnitCommand>, IRequestHandler<DownloadItemUnitCommand, Result>
    {
        private readonly ILangueageService _lang;
        public DownloadItemUnitHandler(ILangueageService lang, IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) :
            base(dbContext, actor, mapper, appCache)
        {
            _lang = lang;
            _lang.EnsureLoad(BulkUploadSettings.BulkUploadCommand);
        }


        public async Task<Result> Handle(DownloadItemUnitCommand request, CancellationToken ct)
        {
            var groups = base.GetAll().ApplyFilters(request.Filter).Select(x => new { x.Id, x.Name}).ToList();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ExcelFiles", "UpdateUnitTemplate.xlsx");
            var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet(1);
            var sheetName = _lang[$"{BulkUploadSettings.BulkUploadCommand}.UpdateUnit.SheetName"];
            ws.Name = sheetName;

            ws.Cells("A1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.Id.ColumnHeader"];
            ws.Cells("B1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.ItemUnitName.ColumnHeader"];
            
            ws.Column("A").Width = 45;
            ws.Column("B").Width = 70;

            ws.Cells("A1:B1").Style.Font.Bold = true;

            var rowNumber = 2;
            foreach(var g in groups)
            {
                var row = ws.Row(rowNumber);
                row.Cell("A").Value = g.Id.ToString();
                row.Cell("B").Value = g.Name?.ToString();
                rowNumber++;
            }

            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return Result.OK(ms);
        }
    }
}
