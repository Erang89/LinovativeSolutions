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
    public class DownloadItemCommand : IRequest<Result>
    {
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class DownloadItemHandler : QueryServiceBase<Item, DownloadItemCommand>, IRequestHandler<DownloadItemCommand, Result>
    {
        private readonly ILangueageService _lang;
        public DownloadItemHandler(ILangueageService lang, IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) :
            base(dbContext, actor, mapper, appCache)
        {
            _lang = lang;
            _lang.EnsureLoad(BulkUploadSettings.BulkUploadCommand);
        }


        public async Task<Result> Handle(DownloadItemCommand request, CancellationToken ct)
        {
            var groups = base.GetAll().ApplyFilters(request.Filter).Select(x => 
                new { 
                    x.Id, 
                    x.Code,
                    x.Name,
                    UnitName = x.Unit!.Name,
                    GroupName = x.Category!.Group!.Name,
                    CategoryName = x.Category.Name,
                    x.SellPrice,
                }).ToList();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ExcelFiles", "UpdateItemTemplate.xlsx");
            var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet(1);
            var sheetName = _lang[$"{BulkUploadSettings.BulkUploadCommand}.UpdateItem.SheetName"];
            ws.Name = sheetName;

            ws.Cells("A1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.Id.ColumnHeader"];
            ws.Cells("B1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.ItemCode.ColumnHeader"];
            ws.Cells("C1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.ItemName.ColumnHeader"];
            ws.Cells("D1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.ItemUnitName.ColumnHeader"];
            ws.Cells("E1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.ItemGroupName.ColumnHeader"];
            ws.Cells("F1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.ItemCategoryName.ColumnHeader"];
            ws.Cells("G1").Value = _lang[$"{BulkUploadSettings.BulkUploadCommand}.ItemSellPrice.ColumnHeader"];

            ws.Column("A").Width = 45;
            ws.Column("B").Width = 30;
            ws.Column("C").Width = 50;
            ws.Column("D").Width = 25;
            ws.Column("E").Width = 25;
            ws.Column("F").Width = 25;
            ws.Column("G").Width = 25;
            
            var rowNumber = 2;
            foreach(var g in groups)
            {
                var row = ws.Row(rowNumber);
                row.Cell("A").Value = g.Id.ToString();
                row.Cell("B").Value = g.Code;
                row.Cell("C").Value = g.Name;
                row.Cell("D").Value = g.UnitName;
                row.Cell("E").Value = g.GroupName;
                row.Cell("F").Value = g.CategoryName;
                row.Cell("G").Value = g.SellPrice;
                rowNumber++;
            }

            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return Result.OK(ms);
        }
    }
}
