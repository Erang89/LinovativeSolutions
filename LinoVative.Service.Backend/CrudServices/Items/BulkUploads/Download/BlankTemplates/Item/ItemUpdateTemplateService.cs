using ClosedXML.Excel;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.BlankTemplates.Item
{
    public class ItemUpdateTemplateService : ExcelTemplateBase
    {
        private readonly ILangueageService _lang;
        public ItemUpdateTemplateService(ILangueageService lang) : base("UpdateItemTemplate", lang, "UpdateItem")
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
            worksheet.Cells("D1").Value = header("ItemUnitName");
            worksheet.Cells("E1").Value = header("ItemGroupName");
            worksheet.Cells("F1").Value = header("ItemCategoryName");
            worksheet.Cells("G1").Value = header("ItemSellPrice");

            return base.FillWorkSheetWithData(worksheet);
        }
    }
}
