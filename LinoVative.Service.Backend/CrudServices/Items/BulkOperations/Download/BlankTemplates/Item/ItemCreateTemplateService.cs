using ClosedXML.Excel;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Item
{
    public class ItemCreateTemplateService : ExcelTemplateBase
    {
        private readonly ILangueageService _lang;
        public ItemCreateTemplateService(ILangueageService lang) : base("CreateItemTemplate", lang, "CreateItem")
        {
            _lang = lang;
        }

        public override Task FillWorkSheetWithData(IXLWorksheet worksheet)
        {
            Func<string, string> header = (key) =>
            {
                return _lang[$"{BulkUploadSettings.BulkUploadCommand}.{key}.ColumnHeader"];
            };

            worksheet.Cells("A1").Value = header("ItemId");
            worksheet.Cells("B1").Value = header("ItemName");
            worksheet.Cells("C1").Value = header("CategoryName");
            worksheet.Cells("D1").Value = header("SKU");
            worksheet.Cells("E1").Value = header("VariantName");
            worksheet.Cells("F1").Value = header("UnitName");
            worksheet.Cells("G1").Value = header("IsActive");
            worksheet.Cells("H1").Value = header("IsPurchaseItem");
            worksheet.Cells("I1").Value = header("IsSaleItem");
            worksheet.Cells("J1").Value = header("SalePrice");
            worksheet.Cells("K1").Value = header("DefaultPurchaseQty");
            worksheet.Cells("L1").Value = header("DefaultMinimumStock");
            worksheet.Cells("M1").Value = header("ItemDescription");

            return base.FillWorkSheetWithData(worksheet);
        }
    }
}
