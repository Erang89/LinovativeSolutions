using ClosedXML.Excel;
using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Factory;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperations.Download.ErrorRecords
{
    public abstract class DownloadErrorItemBase : DownloadErrorRecordBase<ItemBulkUpload, ItemBulkUploadDetail>, IDownloadErrorRecord
    {
        private readonly IBulkOperationTemplateFactory _bulkOperationFactory;
        private readonly ILangueageService _lang;
        protected DownloadErrorItemBase(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory, CrudOperations? operation) : base(dbContext, actor, operation)
        {
            _bulkOperationFactory = templateFactory;
            _lang = lang;
            _lang.EnsureLoad(AvailableLanguageKeys.BulkUploadCommand);
        }

        protected override Task<List<ItemBulkUploadDetail>> GetRecords(Guid uploadId)
        {
            return _dbContext.ItemBulkUploadDetails.Where(x => x.ItemBulkUploadId == uploadId).ToListAsync();
        }

        protected override async Task<IXLWorkbook> GetWorkBook()
        {
            var templateType = _operation switch
            {
                CrudOperations.Create => BulkOperationTemplateType.Item_Create,
                CrudOperations.Update => BulkOperationTemplateType.Item_Update,
                CrudOperations.Delete => BulkOperationTemplateType.Item_Delete,
                _ => BulkOperationTemplateType.Item_Update,
            };
            return await _bulkOperationFactory.GetService(templateType).GetTemplate();
        }

        public Task<MemoryStream> DownloadRows() => base.DownloadDetailRows();

        protected override async Task FillData(IXLWorksheet worksheet)
        {
            var upload = GetUpload();
            var records = upload is null? new List<ItemBulkUploadDetail>() : await GetRecords(upload!.Id);
            if (upload is null) return;

            var row = worksheet.Row(1);
            row.Cell(1).Value = upload.headerColum1;
            row.Cell(2).Value = upload.headerColum2;
            row.Cell(3).Value = upload.headerColum3;
            row.Cell(4).Value = upload.headerColum4;
            row.Cell(5).Value = upload.headerColum5;
            row.Cell(6).Value = upload.headerColum6;
            row.Cell(7).Value = upload.headerColum7;
            row.Cell(8).Value = upload.headerColum8;
            row.Cell(9).Value = upload.headerColum9;
            row.Cell(10).Value = upload.headerColum10;
            row.Cell(11).Value = upload.headerColum11;
            row.Cell(12).Value = upload.headerColum12;
            row.Cell(13).Value = upload.headerColum13;

            row.Cell(1).CopyTo(row.Cell(14));
            row.Cell(14).Value = _lang[$"BulkUploadCommand.Errors.ColumnHeader"];
            worksheet.Column(14).Width = 50;
            worksheet.Column(14).Style.Font.FontColor = XLColor.Red;

            var rowIndex = 2;

            foreach(var data in records)
            {
                row = worksheet.Row(rowIndex);
                row.Cell(1).Value = data.Column1;
                row.Cell(2).Value = data.Column2;
                row.Cell(3).Value = data.Column3;
                row.Cell(4).Value = data.Column4;
                row.Cell(5).Value = data.Column5;
                row.Cell(6).Value = data.Column6;
                row.Cell(7).Value = data.Column7;
                row.Cell(8).Value = data.Column8;
                row.Cell(9).Value = data.Column9;
                row.Cell(10).Value = data.Column10;
                row.Cell(11).Value = data.Column11;
                row.Cell(12).Value = data.Column12;
                row.Cell(13).Value = data.Column13;
                row.Cell(14).Value = data.Errors;

                rowIndex++;
            }
        }
    }

    public class DownloadErrorItemCreateService : DownloadErrorItemBase
    {
        public DownloadErrorItemCreateService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory) 
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Create)
        {
        }
    }

    public class DownloadErrorItemUpdateService : DownloadErrorItemBase
    {
        public DownloadErrorItemUpdateService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory)
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Update)
        {
        }
    }

    public class DownloadErrorItemDeleteService : DownloadErrorItemBase
    {
        public DownloadErrorItemDeleteService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IBulkOperationTemplateFactory templateFactory)
            : base(dbContext, actor, lang, templateFactory, CrudOperations.Delete)
        {
        }
    }
}
