using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group
{


    public abstract class BulkOperationItemGroupBase(ILangueageService lang, IAppDbContext dbContext, IActor actor, CrudOperations crudOperations) : BulkUpdateItemGroupFieldBase
    {



        protected readonly Dictionary<string, Func<string?, object?>> GroupFieldConverters = new()
            {
                {Fields.Id, (x) => StringToGuid(x)},
                {Fields.Name, (x) => x},
            };

        protected readonly Dictionary<string, Func<ItemGroupBulkUploadDetail, string?>> ExcelFieldConverters = new()
            {
                {ExcelColumns.Column1, (x) => x.Column1},
                {ExcelColumns.Column2, (x) => x.Column2},
            };

        protected readonly IAppDbContext _dbContext = dbContext;
        protected readonly IActor _actor = actor;
        protected readonly CrudOperations? _crudOperations = crudOperations;
        protected readonly ILangueageService _lang = lang;

        const string ExcelRowError = "ExcelRowError.Message";
        const string resources = "BulkUploadCommand";

        public virtual async Task<Result> Validate(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token)
        {
            var expectedKeys = new List<string>() { Fields.Id, Fields.Name };
            var expectedColumns = new List<string>() { ExcelColumns.Column1, ExcelColumns.Column2 };
            var bulkUpload = GetBulkUpload();
            var excelRows = GetExcelRows();

            if (bulkUpload is null || excelRows.Count == 0)
                return Result.Failed(GetError("NoRecordUploadedYet.Message"));

            // Validate: is group keys valid
            if (fieldMapping.Any(x => !expectedKeys.Contains(x.Key)))
                return Result.Failed(GetError("InvalidKey.Message"));

            // Validate: is excel keys valid
            if (fieldMapping.Any(x => !expectedColumns.Contains(x.Value)))
                return Result.Failed(GetError("InvalidKey.Message"));

            // Validate: is keyColums valid
            if (_crudOperations is CrudOperations.Update or CrudOperations.Delete && keyColumns.Count == 0)
                return Result.Failed(GetError("NoKeyColumns.Message"));

            if (keyColumns.Count > 0 && keyColumns.Any(x => !fieldMapping.Select(k => k.Key).Contains(x)))
                return Result.Failed(GetError("NoKeyColumns.Message"));

            // Validate: is any fieldMapping exclude keycolumns
            if (keyColumns.Count > 0 && !fieldMapping.Any(x => !keyColumns.Contains(x.Key)))
                return Result.Failed(GetError("NoMappingColumns.Message"));

            return ValidateRows(bulkUpload, fieldMapping, keyColumns);

        }

        Result ValidateRows(ItemGroupBulkUpload bulkUpload, Dictionary<string, string> fieldMapping, List<string> keyColumns)
        {
            var excelRows = GetExcelRows();

            foreach (var key in keyColumns)
            {
                var excelField = fieldMapping[key];
                var cellGetter = ExcelFieldConverters[excelField];
                var converter = GroupFieldConverters[key];
                var inputValues = excelRows.Select(x => converter(cellGetter(x))).Where(x => x != null).ToList();

                var excelColumnName = excelField switch
                {
                    ExcelColumns.Column1 => bulkUpload.headerColum1,
                    ExcelColumns.Column2 => bulkUpload.headerColum2,
                    _ => null
                };


                var validate = ValidateRequiredColumns(excelRows, excelColumnName!, cellGetter);
                if (!validate) return validate;

                validate = ValidateFieldID(key, inputValues, excelRows, cellGetter);
                if (!validate) return validate;

                validate = ValidateGroupName(key, inputValues, excelRows, cellGetter);
                if (!validate) return validate;

            }

            return Result.OK();
        }


        Result ValidateGroupName(string key, List<object?> inputValues, List<ItemGroupBulkUploadDetail> excelRows, Func<ItemGroupBulkUploadDetail, string?> cellGetter)
        {
            var entityName = GetEntityName();
            var fieldName = GetFieldName("ItemGroupName");

            if (key == Fields.Name)
            {
                // Check: Name must exist in the database
                var itemNames = _dbContext.ItemGroups.GetAll(_actor).Where(x => inputValues.Contains(x.Name)).Select(x => x.Name).ToList();
                var notExistNames = inputValues.Where(x => !(itemNames.Select(id => (object?)id)).Contains(x)).ToList();
                var anyError = false;
                foreach (var name in notExistNames)
                {
                    foreach (var row in excelRows.Where(x => cellGetter(x)!.ToString().Equals(name!)))
                    {
                        anyError = true;
                        var cellValue = cellGetter(row);
                        var notFoundIdMessage = string.Format(_lang[$"{resources}.EntityWithFieldNotExist.Message"], entityName, fieldName, cellValue);
                        row.AddError(notFoundIdMessage);
                    }
                }
                if (anyError) return Result.Failed(GetError(ExcelRowError));
            }

            return Result.OK();
        }


        Result ValidateFieldID(string key, List<object?> inputValues, List<ItemGroupBulkUploadDetail> excelRows, Func<ItemGroupBulkUploadDetail, string?> cellGetter)
        {
            var entityName = GetEntityName();

            if (key == Fields.Id)
            {
                var anyError = false;
                var fieldName = GetFieldName("GroupId");

                //Check: input must invalid GUID
                foreach (var row in excelRows.Where(x => !Guid.TryParse(cellGetter(x), out _)))
                {
                    var cellValue = cellGetter(row);
                    var cannotConvertMessage = string.Format(_lang[$"{resources}.CanotCoverValueAsField.Message"], cellValue, fieldName);
                    row.AddError(cannotConvertMessage);
                    anyError = true;
                }

                // Check: ID must exist in the database
                var itemGroupIds = _dbContext.ItemGroups.GetAll(_actor).Where(x => inputValues.Contains(x.Id)).Select(x => x.Id).ToList();
                var notExistIds = inputValues.Where(x => !(itemGroupIds.Select(id => (object?)id)).Contains(x)).ToList();
                foreach (var id in notExistIds)
                {
                    foreach (var row in excelRows.Where(x => cellGetter(x)!.ToString().Equals(id!.ToString())))
                    {
                        var cellValue = cellGetter(row);
                        var notFoundIdMessage = string.Format(_lang[$"{resources}.EntityWithFieldNotExist.Message"], entityName, fieldName, cellValue);
                        row.AddError(notFoundIdMessage);
                        anyError = true;
                    }
                }

                if (anyError) return Result.Failed(GetError(ExcelRowError));
            }

            return Result.OK();
        }


        Result ValidateRequiredColumns(List<ItemGroupBulkUploadDetail> excelRows, string excelColumnName, Func<ItemGroupBulkUploadDetail, string?> cellGetter)
        {
            //Check: Key Column must has value
            var nullValues = excelRows.Where(x => string.IsNullOrWhiteSpace(cellGetter(x)));
            foreach (var row in nullValues)
            {
                row.AddError(GetError("KeyColumnIsEmpty.Message", excelColumnName));
            }
            if (nullValues.Any()) return Result.Failed(GetError(ExcelRowError));

            return Result.OK();
        }


        protected ItemGroupBulkUpload? GetBulkUpload() => _dbContext.ItemGroupBulkUploads.FirstOrDefault(x => x.UserId == _actor.UserId && x.CompanyId == _actor.CompanyId);


        static Guid? StringToGuid(string? str)
        {
            if (str == null) return null;
            if (Guid.TryParse(str, out var id)) return id;
            return null;
        }



        List<ItemGroupBulkUploadDetail>? _excelRows;
        protected List<ItemGroupBulkUploadDetail> GetExcelRows()
        {
            if (_excelRows is not null) return _excelRows;

            _excelRows =
                    [.. _dbContext.ItemGroupBulkUploadDetails.GetAll(_actor).Where(x => x.ItemGroupBulkUpload!.UserId == _actor.UserId && x.ItemGroupBulkUpload.Operation == _crudOperations)];

            foreach (var row in _excelRows)
                row.Errors = null;

            return _excelRows;
        }


        List<ItemGroup>? _groups;
        protected async Task<List<ItemGroup>> GetGroups(Dictionary<string, string> fieldMapping, List<string> keyColumns)
        {
            if (_groups is not null)
                return _groups;

            var excelRows = GetExcelRows();
            var groupQuery = _dbContext.ItemGroups.GetAll(_actor);

            foreach (var key in keyColumns)
            {
                if (key.Equals(Fields.Id, StringComparison.OrdinalIgnoreCase))
                {
                    var inputIds = GetInputValuesFromColumns(excelRows, fieldMapping, key);
                    groupQuery = groupQuery.Where(x => inputIds.Contains(x.Id));
                }

                if (key.Equals(Fields.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var inputNames = GetInputValuesFromColumns(excelRows, fieldMapping, key);
                    groupQuery = groupQuery.Where(x => inputNames.Contains(x.Name));
                }
            }

            _groups = await groupQuery.ToListAsync();
            return _groups;
        }



        protected static ItemGroupBulkUploadDetail? GetRowByGroup(ItemGroup group, List<ItemGroupBulkUploadDetail> rows, Dictionary<string, string> fieldMapping, List<string> keyColumns)
            => GetRowsByGroup(group, rows, fieldMapping, keyColumns).FirstOrDefault();

        protected static IEnumerable<ItemGroupBulkUploadDetail> GetRowsByGroup(ItemGroup group, List<ItemGroupBulkUploadDetail> rows, Dictionary<string, string> fieldMapping, List<string> keyColumns)
        {
            IEnumerable<ItemGroupBulkUploadDetail> selectedRows = rows;
            foreach (var key in keyColumns)
            {
                var excelField = fieldMapping[key]!;

                selectedRows = excelField switch
                {
                    ExcelColumns.Column1 => selectedRows.Where(x => x.Column1!.Equals(GetStringValue(group, key), StringComparison.OrdinalIgnoreCase)),
                    ExcelColumns.Column2 => selectedRows.Where(x => x.Column2!.Equals(GetStringValue(group, key), StringComparison.OrdinalIgnoreCase)),
                    _ => selectedRows
                };
            }
            return selectedRows;
        }

        protected static ItemGroupBulkUploadDetail? GetRowByGroupDto(ItemGroupDto groupDto, List<ItemGroupBulkUploadDetail> rows, Dictionary<string, string> fieldMapping, List<string> keyColumns)
            => GetRowByGroups(groupDto, rows, fieldMapping, keyColumns).FirstOrDefault();

        protected static IEnumerable<ItemGroupBulkUploadDetail> GetRowByGroups(ItemGroupDto group, List<ItemGroupBulkUploadDetail> rows, Dictionary<string, string> fieldMapping, List<string> keyColumns)
        {
            IEnumerable<ItemGroupBulkUploadDetail> selectedRows = rows;
            foreach (var key in keyColumns)
            {
                var excelField = fieldMapping[key]!;

                selectedRows = excelField switch
                {
                    ExcelColumns.Column1 => selectedRows.Where(x => x.Column1 == GetStringValue(group, key)),
                    ExcelColumns.Column2 => selectedRows.Where(x => x.Column2 == GetStringValue(group, key)),
                    _ => selectedRows
                };
            }
            return selectedRows;
        }


        protected static string? GetStringValue(ItemGroup group, string propertyName) =>
            propertyName switch
            {
                Fields.Id => group.Id.ToString(),
                Fields.Name => group.Name,
                _ => null
            };

        protected static string? GetStringValue(ItemGroupDto group, string propertyName) =>
           propertyName switch
           {
               Fields.Id => group.Id.ToString(),
               Fields.Name => group.Name,
               _ => null
           };


        protected List<object?> GetInputValuesFromColumns(List<ItemGroupBulkUploadDetail> excelRows, Dictionary<string, string> fieldMapping, string fieldName)
        {
            var excelFieldName = fieldMapping[fieldName];
            var inputGetter = ExcelFieldConverters[excelFieldName]!;
            var groupFieldConverter = GroupFieldConverters[fieldName]!;
            var values = excelRows.Select(x => groupFieldConverter(inputGetter!(x))).ToList();
            return values;
        }

        protected async Task DeleteBulkUploadRecords()
        {
            await _dbContext.ItemGroupBulkUploadDetails.Where(x =>
                x.ItemGroupBulkUpload!.Operation == _crudOperations &&
                x.ItemGroupBulkUpload.CompanyId == _actor.CompanyId &&
                x.ItemGroupBulkUpload.UserId == _actor.UserId)
                .ExecuteDeleteAsync();

            await _dbContext.ItemGroupBulkUploads.Where(x =>
                x.Operation == _crudOperations &&
                x.CompanyId == _actor.CompanyId &&
                x.UserId == _actor.UserId)
                .ExecuteDeleteAsync();
        }

        protected string GetError(string key, object? value = default) => value is null ? _lang[$"BulkUploadCommand.{key}"] : _lang.Format($"BulkUploadCommand.{key}", value);
        protected string GetFieldName(string key) => _lang[$"BulkUploadCommand.{key}.ColumnHeader"];
        protected string GetEntityName() => _lang[$"BulkUploadCommand.Group.EntityName"];


    }

}
