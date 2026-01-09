using ClosedXML.Excel;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Item
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

            worksheet.Cells("A1").Value = header("SKUId");
            worksheet.Cells("B1").Value = header("ItemName");
            worksheet.Cells("C1").Value = header("SKU");
            worksheet.Cells("D1").Value = header("VariantName");

            return base.FillWorkSheetWithData(worksheet);
        }
    }
}
