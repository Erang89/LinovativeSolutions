using ClosedXML.Excel;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.BlankTemplates.Category
{
    public class CategoryDeleteTemplateService : ExcelTemplateBase
    {
        private readonly ILangueageService _lang;
        public CategoryDeleteTemplateService(ILangueageService lang) : base("DeleteCategoryTemplate", lang, "DeleteCategory")
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
            worksheet.Cells("B1").Value = header("ItemCategoryName");

            return base.FillWorkSheetWithData(worksheet);
        }
    }
}
