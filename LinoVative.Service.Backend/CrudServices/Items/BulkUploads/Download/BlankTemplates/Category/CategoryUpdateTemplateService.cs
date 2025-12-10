using ClosedXML.Excel;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.BlankTemplates.Category
{
    public class CategoryUpdateTemplateService : ExcelTemplateBase
    {
        private readonly ILangueageService _lang;
        public CategoryUpdateTemplateService(ILangueageService lang) : base("UpdateCategoryTemplate", lang, "UpdateCategory")
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
            worksheet.Cells("C1").Value = header("ItemGroupName");

            return base.FillWorkSheetWithData(worksheet);
        }
    }
}
