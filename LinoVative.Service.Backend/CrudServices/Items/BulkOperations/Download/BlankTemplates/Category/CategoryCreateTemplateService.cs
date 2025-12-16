using ClosedXML.Excel;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Category
{
    public class CategoryCreateTemplateService : ExcelTemplateBase
    {
        private readonly ILangueageService _lang;
        public CategoryCreateTemplateService(ILangueageService lang) : base("CreateItemCategoryTemplate", lang, "CreateCategory")
        {
            _lang = lang;
        }

        public override Task FillWorkSheetWithData(IXLWorksheet worksheet)
        {
            Func<string, string> header = (key) =>
            {
                return _lang[$"{BulkUploadSettings.BulkUploadCommand}.{key}.ColumnHeader"];
            };

            worksheet.Cells("A1").Value = header("ItemCategoryName");
            worksheet.Cells("B1").Value = header("ItemGroupName");

            return base.FillWorkSheetWithData(worksheet);
        }
    }
}
