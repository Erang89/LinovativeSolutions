using ClosedXML.Excel;
using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Factory;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperations.Download.ErrorRecords
{
    public abstract class DownloadErrorCategoryBase : DownloadErrorRecordBase<ItemCategoryBulkUpload, ItemCategoryBulkUploadDetail>, IDownloadErrorRecord
    {
        private readonly IBulkOperationTemplateFactory _bulkOperationFactory;
        private readonly ILangueageService _lang;

        protected DownloadErrorCategoryBase(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory, CrudOperations? operation) : base(dbContext, actor, operation)
        {
            _bulkOperationFactory = templateFactory;
            _lang = lang;
            lang.EnsureLoad(x => x.BulkUploadCommand);
        }

        protected override Task<List<ItemCategoryBulkUploadDetail>> GetRecords(Guid uploadId)
        {
            return _dbContext.ItemCategoryBulkUploadDetails.Where(x => x.ItemCategoryBulkUploadId == uploadId).ToListAsync();
        }

        protected override async Task<IXLWorkbook> GetWorkBook()
        {
            var templateType = _operation switch
            {
                CrudOperations.Create => BulkOperationTemplateType.Category_Create,
                CrudOperations.Update => BulkOperationTemplateType.Category_Update,
                CrudOperations.Delete => BulkOperationTemplateType.Category_Delete,
                _ => BulkOperationTemplateType.Category_Update,
            };
            return await _bulkOperationFactory.GetService(templateType).GetTemplate();
        }

        public Task<MemoryStream> DownloadRows() => base.DownloadDetailRows();

        protected override async Task FillData(IXLWorksheet worksheet)
        {
            var upload = GetUpload();
            var records = upload is null? new List<ItemCategoryBulkUploadDetail>() : await GetRecords(upload!.Id);
            if (upload is null) return;

            var row = worksheet.Row(1);
            row.Cell(1).Value = upload.headerColum1;
            row.Cell(2).Value = upload.headerColum2;
            row.Cell(3).Value = upload.headerColum3;
            row.Cell(4).Value = _lang[$"BulkUploadCommand.Errors.ColumnHeader"];

            var rowIndex = 2;

            foreach(var data in records)
            {
                row = worksheet.Row(rowIndex);
                row.Cell(1).Value = data.Column1;
                row.Cell(2).Value = data.Column2;
                row.Cell(3).Value = data.Column3;
                row.Cell(4).Value = data.Errors;
            }
        }
    }

    public class DownloadErrorCategooryCreateService : DownloadErrorCategoryBase
    {
        public DownloadErrorCategooryCreateService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory) 
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Create)
        {
        }
    }

    public class DownloadErrorCategoryUpdateService : DownloadErrorCategoryBase
    {
        public DownloadErrorCategoryUpdateService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory)
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Update)
        {
        }
    }

    public class DownloadErrorCategoryDeleteService : DownloadErrorCategoryBase
    {
        public DownloadErrorCategoryDeleteService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory)
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Delete)
        {
        }
    }
}
