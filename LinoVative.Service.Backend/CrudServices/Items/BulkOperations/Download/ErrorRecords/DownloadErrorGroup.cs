using ClosedXML.Excel;
using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Factory;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperations.Download.ErrorRecords
{
    public abstract class DownloadErrorGroupBase : DownloadErrorRecordBase<ItemGroupBulkUpload, ItemGroupBulkUploadDetail>, IDownloadErrorRecord
    {
        private readonly IBulkOperationTemplateFactory _bulkOperationFactory;
        private readonly ILangueageService _lang;
        protected DownloadErrorGroupBase(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory, CrudOperations? operation) : base(dbContext, actor, operation)
        {
            _bulkOperationFactory = templateFactory;
            _lang = lang;
            _lang.EnsureLoad(x => x.BulkUploadCommand);
        }

        protected override Task<List<ItemGroupBulkUploadDetail>> GetRecords(Guid uploadId)
        {
            return _dbContext.ItemGroupBulkUploadDetails.Where(x => x.ItemGroupBulkUploadId == uploadId).ToListAsync();
        }

        protected override async Task<IXLWorkbook> GetWorkBook()
        {
            var templateType = _operation switch
            {
                CrudOperations.Create => BulkOperationTemplateType.Group_Create,
                CrudOperations.Update => BulkOperationTemplateType.Group_Update,
                CrudOperations.Delete => BulkOperationTemplateType.Group_Delete,
                _ => BulkOperationTemplateType.Group_Update,
            };
            return await _bulkOperationFactory.GetService(templateType).GetTemplate();
        }

        public Task<MemoryStream> DownloadRows() => base.DownloadDetailRows();

        protected override async Task FillData(IXLWorksheet worksheet)
        {
            var upload = GetUpload();
            var records = upload is null? new List<ItemGroupBulkUploadDetail>() : await GetRecords(upload!.Id);
            if (upload is null) return;

            var row = worksheet.Row(1);
            row.Cell(1).Value = upload.headerColum1;
            row.Cell(2).Value = upload.headerColum2;
            row.Cell(3).Value = _lang[$"BulkUploadCommand.Errors.ColumnHeader"];

            var rowIndex = 2;

            foreach(var data in records)
            {
                row = worksheet.Row(rowIndex);
                row.Cell(1).Value = data.Column1;
                row.Cell(2).Value = data.Column2;
                row.Cell(3).Value = data.Errors;
            }
        }
    }

    public class DownloadErrorGroupCreateService : DownloadErrorGroupBase
    {
        public DownloadErrorGroupCreateService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory) 
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Create)
        {
        }
    }

    public class DownloadErrorGroupUpdateService : DownloadErrorGroupBase
    {
        public DownloadErrorGroupUpdateService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory)
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Update)
        {
        }
    }

    public class DownloadErrorGroupDeleteService : DownloadErrorGroupBase
    {
        public DownloadErrorGroupDeleteService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory)
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Delete)
        {
        }
    }
}
