using ClosedXML.Excel;
using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads
{
    public class BulkUploadItemUnitCommand : IRequest<Result>
    {
        public IFormFile? File { get; set; }
        public CrudOperations? Operation { get; set; } = CrudOperations.Create;
    }

    public class BulkUploadItemUnitService : IRequestHandler<BulkUploadItemUnitCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ILangueageService _lang;
        private readonly IActor _actor;
        private IXLWorkbook? _workBook;

        public BulkUploadItemUnitService(IAppDbContext db, ILangueageService lang, IActor actor)
        {
            _dbContext = db;
            _lang = lang;
            _actor = actor;
           _lang.EnsureLoad(BulkUploadSettings.BulkUploadCommand);
        }


        public async Task<Result> Handle(BulkUploadItemUnitCommand request, CancellationToken ct)
        {
            var validate = await Validate(request, ct);
            if (!validate)
            {
                _workBook?.Dispose();
                return validate;
            }

            var worksheet = (await GetWorksheet(request, ct))!;
            var exisitingUpload = GetBulkRecord(request);
            if(exisitingUpload is null)
            {
                var header = worksheet.Row(1);
                var header1 = GetValueString(header.Cell(1));
                var header2 = GetValueString(header.Cell(2));
                exisitingUpload = new ItemUnitBulkUpload() { headerColum1 = header1, headerColum2 = header2, CompanyId = _actor.CompanyId, UserId = _actor.UserId, Operation = request.Operation};
                _dbContext.ItemUnitBulkUploads.Add(exisitingUpload);
            }

            var totalRows = worksheet.RowsUsed().Count() - 1;
            for(var i = 2; i<= totalRows; i++)
            {
                var row = worksheet.Row(i);
                var detail = new ItemUnitBulkUploadDetail()
                {
                    ItemUnitBulkUploadId = exisitingUpload.Id,
                    Column1 = GetValueString(row.Cell(1)),
                    Column2 = GetValueString(row.Cell(2)),
                };

                _dbContext.ItemUnitBulkUploadDetails.Add(detail);
            }

            await _dbContext.SaveAsync(_actor);
            _workBook?.Dispose();
            return Result.OK(exisitingUpload.Id);
        }


        string? GetValueString(IXLCell cell)
        {

            var type = cell.DataType;
            string? value = null;
            if(string.IsNullOrEmpty(cell.GetString())) return null;

            if (type == XLDataType.DateTime)
            {
                var valDate = cell.GetDateTime();
                value = valDate.ToString("yyyy-MM-dd");
            }
            else if (cell is not null)
            {
                value = cell.GetString();
            }

            return value;
        }

        IXLWorksheet? workSheet = null;
        async Task<IXLWorksheet?> GetWorksheet(BulkUploadItemUnitCommand request, CancellationToken ct)
        {
            if(workSheet is not null) return workSheet;
            using var ms = new MemoryStream();
            await request.File!.CopyToAsync(ms, ct);
            ms.Position = 0;

            _workBook = new XLWorkbook(ms);
            workSheet = _workBook.Worksheets.FirstOrDefault();
            await Task.CompletedTask;
            return workSheet;
        }


        ItemUnitBulkUpload? bulkRecord = null;
        ItemUnitBulkUpload? GetBulkRecord(BulkUploadItemUnitCommand request)
        {
            if(bulkRecord is not null)
                return bulkRecord;

            bulkRecord = _dbContext.ItemUnitBulkUploads.FirstOrDefault(x => x.UserId == _actor.UserId && x.CompanyId == _actor.CompanyId && !x.IsDeleted && x.Operation == request.Operation);
            return bulkRecord;
        }

        async Task<Result> Validate(BulkUploadItemUnitCommand request, CancellationToken ct)
        {
            var file = request.File;

            if (file == null)
            {
                var message = _lang[$"{BulkUploadSettings.BulkUploadCommand}.NoFileUpload"];
                return Result.Failed(message);
            }

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (ext != ".xlsx")
            {
                var message = _lang[$"{BulkUploadSettings.BulkUploadCommand}.OnlyExcelFileSupported"];
                return Result.Failed(message);
            }

            if (file.Length == 0)
            {
                var message = _lang[$"{BulkUploadSettings.BulkUploadCommand}.UploadFileEmplty"];
                return Result.Failed(message);
            }

            var worksheet = await GetWorksheet(request, ct);
            if (worksheet == null)
                return Result.Failed(_lang[$"{BulkUploadSettings.BulkUploadCommand}.NoWorksheetFound"]);

            var existingUpload = GetBulkRecord(request);
            if (existingUpload != null)
            {
                var header = worksheet.Row(1);
                var header1 = GetValueString(header.Cell(1));
                var header2 = GetValueString(header.Cell(2));
                if (header1?.ToLower() != existingUpload.headerColum1?.ToLower() || header2?.ToLower() != existingUpload.headerColum2?.ToLower())
                    return Result.Failed(_lang[$"{BulkUploadSettings.BulkUploadCommand}.InvalidHeader"]);
            }

            await Task.CompletedTask;
            return Result.OK();
        }
    }
}
