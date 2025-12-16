using ClosedXML.Excel;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Unit
{
    public class UnitCreateTemplateService : ExcelTemplateBase
    {
        private readonly ILangueageService _lang;
        public UnitCreateTemplateService(ILangueageService lang) : base("CreateItemUnitTemplate", lang, "CreateUnit")
        {
            _lang = lang;
        }

        public override Task FillWorkSheetWithData(IXLWorksheet worksheet)
        {
            Func<string, string> header = (key) =>
            {
                return _lang[$"{BulkUploadSettings.BulkUploadCommand}.{key}.ColumnHeader"];
            };

            worksheet.Cells("A1").Value = header("ItemUnitName");

            return base.FillWorkSheetWithData(worksheet);
        }
    }
}
