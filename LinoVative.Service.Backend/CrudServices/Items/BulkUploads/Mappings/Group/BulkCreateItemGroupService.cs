using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group
{
    public class BulkCreateItemGroupService : BulkUpdateItemGroupBase, IBulkMapping
    {
        const string ExcelRowError = "ExcelRowError.Message";

        public BulkCreateItemGroupService(ILangueageService lang, IAppDbContext dbContext, IActor actor) : base(lang, dbContext, actor, CrudOperations.Create)
        {

        }

        public async Task<Result> Save(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token)
        {
            var validate = await Validate(fieldMapping, keyColumns, token);
            if (!validate) return validate;

            var newGroups = new List<ItemGroup>();

            var excelRows = GetExcelRows();
            foreach (var row in excelRows)
            {

                var group = new ItemGroup();

                foreach (var map in fieldMapping!.ToList())
                {
                    var key = map.Key;
                    var excelField = fieldMapping[key];
                    var cellGetter = ExcelFieldConverters[excelField];
                    var converter = GroupFieldConverters[key];

                    if (key == Fields.Id && cellGetter(row) != null)
                        group.Id = (Guid)converter(cellGetter(row))!;

                    else if (key == Fields.Name)
                        group.Name = (string)converter(cellGetter(row))!;

                }

                group.CompanyId = _actor.CompanyId;
                group.CreateBy(_actor);
                newGroups.Add(group);

            }

            _dbContext.ItemGroups.AddRange(newGroups);
            var result = await _dbContext.SaveAsync(_actor);
            await DeleteBulkUploadRecords();
            return Result.OK();
        }



        public override async Task<Result> Validate(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token)
        {
            var validate = await base.Validate(fieldMapping, keyColumns, token);
            if (!validate) return validate;

            Func<string, object?, string> getError = (key, value) => value is null ? _lang[$"BulkUploadCommand.{key}"] : _lang.Format($"BulkUploadCommand.{key}", value);
            var bulkUpload = GetBulkUpload()!;

            if (keyColumns.Count > 0)
                return Result.Failed(getError("NoKeyColumnsNeeded.Message", null));

            foreach (var mapping in fieldMapping!.ToList())
            {
                var key = mapping.Key;
                var excelField = fieldMapping[key];
                var cellGetter = ExcelFieldConverters[excelField];
                var converter = GroupFieldConverters[key];
                var excelColumnName = excelField switch
                {
                    ExcelColumns.Column1 => bulkUpload.headerColum1,
                    ExcelColumns.Column2 => bulkUpload.headerColum2,
                    _ => null
                };


                var validateId = await ValidateId(key!, cellGetter!, converter!, getError!, excelColumnName!);
                var validateName = await ValidateName(key!, cellGetter!, converter!, getError!, excelColumnName!);
                
                if (!validateId) return validateId;
                if (!validateName) return validateName;

            }

            return Result.OK();
        }


        private async Task<Result> ValidateId(string key, Func<ItemGroupBulkUploadDetail, string?> cellGetter, Func<string?, object?> converter, Func<string, object?, string> getError, string excelColumnName)
        {

            var excelRows = GetExcelRows();

            if (key == Fields.Id)
            {
                var inputIds = excelRows.Select(x => converter(cellGetter(x))).Where(x => x != null).ToList();

                // Check Duplicate ID from excel
                var groupingById = excelRows.GroupBy(x => cellGetter(x)).Select(x => new { Id = x.Key, Count = x.Count(), rows = x.ToList() });
                var duplicateRows = groupingById.Where(x => x.Count > 1).SelectMany(x => x.rows).ToList() ?? new();
                foreach (var rows in duplicateRows)
                {
                    rows.Errors = getError("DuplicateIdInExcel.Message", excelColumnName);
                }
                if (duplicateRows.Any()) return Result.Failed(getError(ExcelRowError, null));


                // Check if ID already exist in database
                var idExist = _dbContext.ItemGroups.Where(x => inputIds.Contains(x.Id)).Select(x => x.Id).Take(100).ToList();
                foreach (var row in excelRows.Where(x => idExist.Select(x => x.ToString()).Contains(cellGetter(x))))
                {
                    row.Errors = getError("DuplicateIdInDB.Message", excelColumnName);
                }
                if (idExist.Any()) return Result.Failed(getError(ExcelRowError, null));
            }

            await Task.CompletedTask;
            return Result.OK();
        }


        private async Task<Result> ValidateName(string key, Func<ItemGroupBulkUploadDetail, string?> cellGetter, Func<string?, object?> converter, Func<string, object?, string> getError, string excelColumnName)
        {
            var excelRows = GetExcelRows();

            if (key == Fields.Name)
            {
                // Check: Any null values
                var emptyNames = excelRows.Where(x => cellGetter(x) == null);
                foreach (var row in emptyNames)
                {
                    row.Errors = getError("GroupNameRequired.Message", null);
                }
                if (emptyNames.Any()) return Result.Failed(getError("ExcelRowError.Message", null));


                var inputNames = excelRows.Select(x => converter(cellGetter(x))).Where(x => x != null).ToList();


                // Check duplicate name from excel
                var groupNames = excelRows.GroupBy(x => cellGetter(x)!.ToLower()).Select(x => new { Id = x.Key, Count = x.Count(), rows = x.ToList() });
                var duplicateRows = groupNames.Where(x => x.Count > 1).SelectMany(x => x.rows).ToList() ?? new();
                foreach (var rows in duplicateRows)
                {
                    rows.Errors = getError("DuplicateNameInExcel.Message", excelColumnName);
                }
                if (duplicateRows.Any()) return Result.Failed(getError(ExcelRowError, null));

                // Check if Name already exist in database
                var nameExist = _dbContext.ItemGroups.Where(x => inputNames.Contains(x.Name)).Select(x => x.Name).Take(100).ToList();
                foreach (var row in excelRows.Where(x => nameExist.Select(x => x).Contains(cellGetter(x))))
                {
                    row.Errors = getError("DuplicateNameInDB.Message", excelColumnName);
                }
                if (nameExist.Any()) return Result.Failed(getError("ExcelRowError.Message", null));
            }

            await Task.CompletedTask;
            return Result.OK();
        }
    }
}
