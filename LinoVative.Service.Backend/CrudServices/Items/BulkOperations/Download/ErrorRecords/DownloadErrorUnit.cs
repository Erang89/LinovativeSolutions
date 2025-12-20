using ClosedXML.Excel;
using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Factory;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperations.Download.ErrorRecords
{
    public abstract class DownloadErrorUnitBase : DownloadErrorRecordBase<ItemUnitBulkUpload, ItemUnitBulkUploadDetail>, IDownloadErrorRecord
    {
        private readonly IBulkOperationTemplateFactory _bulkOperationFactory;
        private readonly ILangueageService _lang;
        protected DownloadErrorUnitBase(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory, CrudOperations? operation) : base(dbContext, actor, operation)
        {
            _bulkOperationFactory = templateFactory;
            _lang = lang;
            _lang.EnsureLoad(AvailableLanguageKeys.BulkUploadCommand);
        }

        protected override Task<List<ItemUnitBulkUploadDetail>> GetRecords(Guid uploadId)
        {
            return _dbContext.ItemUnitBulkUploadDetails.Where(x => x.ItemUnitBulkUploadId == uploadId).ToListAsync();
        }

        protected override async Task<IXLWorkbook> GetWorkBook()
        {
            var templateType = _operation switch
            {
                CrudOperations.Create => BulkOperationTemplateType.Unit_Create,
                CrudOperations.Update => BulkOperationTemplateType.Unit_Update,
                CrudOperations.Delete => BulkOperationTemplateType.Unit_Delete,
                _ => BulkOperationTemplateType.Unit_Update,
            };
            return await _bulkOperationFactory.GetService(templateType).GetTemplate();
        }

        public Task<MemoryStream> DownloadRows() => base.DownloadDetailRows();

        protected override async Task FillData(IXLWorksheet worksheet)
        {
            var upload = GetUpload();
            var records = upload is null? new List<ItemUnitBulkUploadDetail>() : await GetRecords(upload!.Id);
            if (upload is null) return;

            var row = worksheet.Row(1);
            row.Cell(1).Value = upload.headerColum1;
            row.Cell(2).Value = upload.headerColum2;
            row.Cell(1).CopyTo(row.Cell(3));
            row.Cell(3).Value = _lang[$"BulkUploadCommand.Errors.ColumnHeader"];
            worksheet.Column(3).Width = 50;
            worksheet.Column(3).Style.Font.FontColor = XLColor.Red;

            var rowIndex = 2;

            foreach(var data in records)
            {
                row = worksheet.Row(rowIndex);
                row.Cell(1).Value = data.Column1;
                row.Cell(2).Value = data.Column2;
                row.Cell(3).Value = data.Errors;

                rowIndex++;
            }
        }
    }

    public class DownloadErrorUnitCreateService : DownloadErrorUnitBase
    {
        public DownloadErrorUnitCreateService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory) 
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Create)
        {
        }

    }

    public class DownloadErrorUnitUpdateService : DownloadErrorUnitBase
    {
        public DownloadErrorUnitUpdateService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory)
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Update)
        {
        }
    }

    public class DownloadErrorUnitDeleteService : DownloadErrorUnitBase
    {
        public DownloadErrorUnitDeleteService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory)
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Delete)
        {
        }

    }
}
