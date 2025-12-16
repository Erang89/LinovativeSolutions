using ClosedXML.Excel;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Base
{
    public interface IExcelTemplateService
    {
        public Task<XLWorkbook> GetTemplate();
        public Task<MemoryStream> GetTemplateMemoryStream();
    }




    public abstract class ExcelTemplateBase : IExcelTemplateService
    {
        private readonly string TemplateName;
        private readonly string WorkSheetName;

        protected ExcelTemplateBase(string templateName, ILangueageService lang, string sheetNameResourceKey)
        {
            TemplateName = templateName;
            lang.EnsureLoad(BulkUploadSettings.BulkUploadCommand);
            WorkSheetName = lang[$"{BulkUploadSettings.BulkUploadCommand}.{sheetNameResourceKey}.SheetName"];
        }

        public async Task<XLWorkbook> GetTemplate()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ExcelFiles", $"{TemplateName}.xlsx");
            var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet(1);
            ws.Name = WorkSheetName;
            await FillWorkSheetWithData(ws);
            return wb;
        }


        public virtual async Task FillWorkSheetWithData(IXLWorksheet worksheet) { }

        public async Task<MemoryStream> GetTemplateMemoryStream()
        {
            var wb = await GetTemplate();
            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return ms;
        }
    }
}
