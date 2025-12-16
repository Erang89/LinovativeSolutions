using ClosedXML.Excel;
using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperations.Download.ErrorRecords
{
    
    public abstract class DownloadErrorRecordBase<TUpload, TRecord>
        where TUpload : class, IExcelBulkUpload, IEntityId, IsEntityManageByCompany
        where TRecord : class, IBuklUploadDetail
    {
        protected readonly IAppDbContext _dbContext;
        protected DbSet<TUpload> _uploads;
        protected DbSet<TRecord> _records;
        protected readonly CrudOperations? _operation;
        protected readonly IActor _actor;

        protected DownloadErrorRecordBase(IAppDbContext dbContext, IActor actor, CrudOperations? operation)
        {
            _dbContext = dbContext;
            _uploads = dbContext.Set<TUpload>();
            _records = dbContext.Set<TRecord>();
            _operation = operation;
            _actor = actor;
        }

        protected abstract Task<List<TRecord>> GetRecords(Guid uploadId);
        protected abstract Task<IXLWorkbook> GetWorkBook();
        protected abstract Task FillData(IXLWorksheet worksheet);

        protected TUpload? GetUpload() => _uploads.GetAll(_actor).Where(x => x.UserId == _actor.UserId && x.Operation == _operation).FirstOrDefault();

        public async Task<MemoryStream> DownloadDetailRows()
        {
            var wb = await GetWorkBook();
            var ws = wb.Worksheet(1);
            await FillData(ws);
            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return ms;
        }
    }
}
