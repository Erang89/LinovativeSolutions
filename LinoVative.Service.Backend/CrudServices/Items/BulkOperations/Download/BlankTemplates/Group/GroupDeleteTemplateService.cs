using ClosedXML.Excel;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Group
{
    public class GroupDeleteTemplateService : ExcelTemplateBase
    {
        private readonly ILangueageService _lang;
        public GroupDeleteTemplateService(ILangueageService lang) : base("DeleteGroupTemplate", lang, "DeleteGroup")
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
            worksheet.Cells("B1").Value = header("ItemGroupName");

            return base.FillWorkSheetWithData(worksheet);
        }
    }
}
