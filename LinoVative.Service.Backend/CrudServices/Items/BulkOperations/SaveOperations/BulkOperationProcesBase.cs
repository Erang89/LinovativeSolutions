using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations
{
    public abstract class BulkOperationProcesBase<TUpload, TRecord> : IBulkOperationProcess
        where TUpload : class, IExcelBulkUpload, IEntityId, IsEntityManageByCompany
        where TRecord : class, IBuklUploadDetail
    {

        // Const
        protected const string _resources = "BulkUploadCommand";
        protected const string _excelRowError = "ExcelRowError.Message";
        protected const string _columnId = "Id";


        // Abstracts
        protected abstract IDictionary<string, Func<string?, object?>> Converters { get; }
        protected abstract IDictionary<string, Func<TRecord, string?>> Getters { get; }
        protected abstract string? GetExcelHeader(string key);
        protected abstract string EntityKey { get; }
        protected abstract bool IsValidFields();
        protected abstract Task BulkOperationHandler(CancellationToken token);
        protected abstract IQueryable<TRecord> FilterRecords(IQueryable<TRecord> query, Guid? uploadId);
        protected abstract bool IdsShouldExist(IEnumerable<Guid> ids);
        protected abstract bool IdsShouldNOTExist(IEnumerable<Guid> ids);
        

        // Properties
        protected readonly IAppDbContext _dbContext;
        protected readonly ILangueageService _lang;
        protected readonly IActor _actor;
        protected DbSet<TUpload> _bulkUploads;
        protected DbSet<TRecord> _bulkUploadDetails;
        protected CrudOperations? _operation;
        protected Dictionary<string, string> _fieldMapping = [];
        

        // Constructor
        protected BulkOperationProcesBase(IAppDbContext dbContext, IActor actor, ILangueageService lang, CrudOperations operation)
        {
            _dbContext = dbContext;
            _actor = actor;
            _lang = lang;
            _bulkUploads = _dbContext.Set<TUpload>();
            _bulkUploadDetails = _dbContext.Set<TRecord>();
            _operation = operation;
            lang.EnsureLoad(x => x.BulkUploadCommand);
        }


        // Protected Functions
        #region Protected Functions

        protected virtual string GetEntityName() => _lang[$"BulkUploadCommand.{EntityKey}.EntityName"];
        protected virtual string GetFieldName(string key) => _lang[$"BulkUploadCommand.{key}.ColumnHeader"];
        protected virtual List<string> RequieredFieldWhenCreated { get; } = [];


        TUpload? _bulkUpload = null;
        protected TUpload? GetBulkUpload()
        {
            if (_bulkUpload != null) return _bulkUpload;

            _bulkUpload = _bulkUploads.GetAll(_actor).FirstOrDefault(x => x.UserId == _actor.UserId && x.CompanyId == _actor.CompanyId && x.Operation == _operation);
            return _bulkUpload;
        }

        List<TRecord>? _records = null;
        protected List<TRecord> GetRecords()
        {
            if (_records != null) return _records;

            var parent = GetBulkUpload();
            _records = [.. FilterRecords(_bulkUploadDetails.GetAll(_actor), parent?.Id)];
            return _records;
        }

        protected List<object?> GetInputValues(string key)
        {
            var excelColumn = _fieldMapping[key];
            var cellGetter = Getters[excelColumn];
            var converter = Converters[key];
            var excelRows = GetRecords();
            return [.. excelRows.Select(x => converter(cellGetter(x))).Where(x => x != null)];
        }

        protected (Func<TRecord, string?>, Func<string?, object?>) GetGetterAndConverter(string key)
        {
            if (!_fieldMapping.TryGetValue(key, out string? excelColumn)) return ((TRecord) => null, (_) => null);
            var cellGetter = Getters[excelColumn];
            var converter = Converters[key];
            return (cellGetter, converter);
        }
        
        protected string GetError(string key, object? value = default) => value is null ? _lang[$"{_resources}.{key}"] : _lang.Format($"{_resources}.{key}", value);

        #endregion





        // Private Functions
        #region Private Functions
        bool CheckRequiredId()
        {
            if (_operation == CrudOperations.Create) return true;

            var excelRows = GetRecords();

            var excelField = _fieldMapping[_columnId];
            var cellGetter = Getters[excelField];
            var excelColumnName = GetExcelHeader(excelField);

            var nullValues = excelRows.Where(x => string.IsNullOrWhiteSpace(cellGetter(x)));
            foreach (var row in nullValues)
            {
                row.AddError(GetError("KeyColumnIsEmpty.Message", excelColumnName));
            }

            return !nullValues.Any();
        }
        

        bool CheckInputId()
        {
            if (!_fieldMapping.Any(x => x.Key == _columnId))
                return true;

            var anyError = false;
            var (cellGetter, _) = GetGetterAndConverter(_columnId);
           
            foreach (var row in GetRecords().Where(x => !string.IsNullOrEmpty(cellGetter(x)) && !Guid.TryParse(cellGetter(x), out _)))
            {
                var cellValue = cellGetter(row);
                var cannotConvertMessage = string.Format(_lang[$"{_resources}.CanotCoverValueAsField.Message"], cellValue, GetFieldName(_columnId));
                row.AddError(cannotConvertMessage);
                anyError = true;
            }

            return !anyError;
        }

        bool CheckDuplicateInputId()
        {
            if (!_fieldMapping.Any(x => x.Key == _columnId))
                return true;

            var anyError = false;
            var (cellGetter, _) = GetGetterAndConverter(_columnId);
            var idGroups = GetRecords().Where(x => !string.IsNullOrWhiteSpace(cellGetter(x))).
                GroupBy(x => cellGetter(x)!.ToLower())
                .Select(x => new { x.Key, Count = x.Count(), Rows = x});

            foreach(var row in idGroups.Where(x => x.Count > 1).SelectMany(x => x.Rows))
            {
                row.AddError(string.Format(_lang[$"{_resources}.DuplicateIdInExcel.Message"], cellGetter(row)));
                anyError = true;
            }

            return !anyError;

        }

        bool IsValidID()
        {
            var checkRequired = CheckRequiredId();
            var checkInput = CheckInputId();
            var checkDuplicateInput = CheckDuplicateInputId();
            var checkExistOrNotExisitId = true;
            if (_fieldMapping.ContainsKey(_columnId))
                checkExistOrNotExisitId = 
                _operation == CrudOperations.Create ? 
                IdsShouldNOTExist(GetInputValues(_columnId).Select(x => (Guid)x!)) :
                IdsShouldExist(GetInputValues(_columnId).Select(x => (Guid)x!));

            return checkRequired && checkInput && checkDuplicateInput && checkExistOrNotExisitId;
        }

        async Task DeleteBulkUploadRecords()
        {
            var bulk = GetBulkUpload()!;
            await _bulkUploads.Where(x => x.Id == bulk!.Id).ExecuteDeleteAsync();
            await FilterRecords(_bulkUploadDetails.GetAll(_actor), bulk.Id).ExecuteDeleteAsync();
        }

        #endregion





        // Public Functions
        #region Public Functions
        public async Task<Result> Save(Dictionary<string, string> fieldMapping, CancellationToken token)
        {
            _fieldMapping = fieldMapping;

            var validate = await Validate(fieldMapping, token);
            if (!validate)
            {
                await _dbContext.SaveAsync(_actor, token);
                return validate;
            }

            await BulkOperationHandler(token);

            await _dbContext.SaveAsync(_actor, token);
            await DeleteBulkUploadRecords();
            return Result.OK();
        }

        public virtual async Task<Result> Validate(Dictionary<string, string> fieldMapping,  CancellationToken token)
        {
            _fieldMapping = fieldMapping;
            var expectedKeys = Converters.Select(x => x.Key);
            var expectedColumns = Getters.Select(x => x.Key);

            var bulkUpload = GetBulkUpload();
            var excelRows = GetRecords();

            foreach(var row in excelRows) row.ClearError();

            if (bulkUpload is null || excelRows.Count == 0)
                return Result.Failed(GetError("NoRecordUploadedYet.Message"));

            // Validate: Field mapping key shoud in expected field
            if (fieldMapping.Any(x => !expectedKeys.Contains(x.Key)))
                return Result.Failed(GetError("InvalidKey.Message"));

            // Validate: Field mapping value shoud in expected excel column
            if (fieldMapping.Any(x => !expectedColumns.Contains(x.Value)))
                return Result.Failed(GetError("InvalidKey.Message"));

            // Validate: Key Columns shoud selected when operation is 'CREATED' or 'UPDATE'
            if (_operation is CrudOperations.Update or CrudOperations.Delete && !_fieldMapping.Any(x => x.Key == _columnId))
                return Result.Failed(GetError("NoKeyColumns.Message"));

            // Validate: Should have at least one colum mapping other then ID
            if (!_fieldMapping.Any(x => x.Key != _columnId))
                return Result.Failed(GetError("NoMappingColumns.Message"));

            // Check Required Filed
            if(RequieredFieldWhenCreated.Count > 0 && RequieredFieldWhenCreated.Any(x => !_fieldMapping.Keys.Contains(x)))
                return Result.Failed(GetError("FieldsRequired.Message", string.Join(", ", RequieredFieldWhenCreated.Select(x => $"{x}.ColumnHeader").Select(x => GetError(x)))));


            var isValidId = IsValidID();
            var isValidFields = IsValidFields();
            var isValid = isValidId && isValidFields;
            var errorMessage = GetError(_excelRowError);
            return isValid ? Result.OK() : Result.Failed(errorMessage);
        }

        #endregion


    }
}
