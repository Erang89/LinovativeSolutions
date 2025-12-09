using ClosedXML.Excel;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.BlankTemplates.Item
{
    public class ItemDeleteTemplateService : ExcelTemplateBase
    {
        private readonly ILangueageService _lang;
        public ItemDeleteTemplateService(ILangueageService lang) : base("DeleteItemTemplate", lang, "DeleteItem")
        {
            _lang = lang;
        }

        public override Task FillWorkSheetWithData(IXLWorksheet worksheet)
        {
            Func<string, string> header = (key) =>
            {
                return _lang[$"{BulkUploadSettings.BulkUploadCommand}.{key}.ColumnHeader"];
            };

            worksheet.Cells("A1").Value = header("Id");
            worksheet.Cells("B1").Value = header("ItemCode");
            worksheet.Cells("C1").Value = header("ItemName");

            return base.FillWorkSheetWithData(worksheet);
        }
    }
}
