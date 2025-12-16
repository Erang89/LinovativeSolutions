using ClosedXML.Excel;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Unit
{
    public class UnitUpdateTemplateService : ExcelTemplateBase
    {
        private readonly ILangueageService _lang;
        public UnitUpdateTemplateService(ILangueageService lang) : base("UpdateUnitTemplate", lang, "UpdateUnit")
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
            worksheet.Cells("B1").Value = header("ItemUnitName");

            return base.FillWorkSheetWithData(worksheet);
        }
    }
}
