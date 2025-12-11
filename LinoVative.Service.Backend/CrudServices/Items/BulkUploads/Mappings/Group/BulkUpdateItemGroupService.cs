using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group
{
    public class BulkUpdateItemGroupService(ILangueageService lang, IAppDbContext dbContext, IActor actor) : BulkUpdateItemGroupBase(lang, dbContext, actor, CrudOperations.Update), IBulkMapping
    {
        public async Task<Result> Save(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token)
        {
            var validate = await Validate(fieldMapping, keyColumns, token);

            if (!validate)
            {
                await _dbContext.SaveAsync(_actor, token);
                return validate;
            }

            var groups = await GetGroups(fieldMapping, keyColumns);
            var rows = GetExcelRows();


            foreach (var group in groups)
            {
                var row = GetRowByGroup(group, rows, fieldMapping, keyColumns);
                if (row is null) continue;

                MappingGroup(group, row, fieldMapping, keyColumns);
            }

            await _dbContext.SaveAsync(_actor, token);
            await DeleteBulkUploadRecords();

            return Result.OK();
        }




        public override async Task<Result> Validate(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token)
        {

            var validate = await base.Validate(fieldMapping, keyColumns, token);
            if (!validate) return validate;

            var groups = await GetGroups(fieldMapping, keyColumns);
            var groupDtos = groups.Select(x => new ItemGroupDto() { Id = x.Id, Name = x.Name }).ToList();
            var dtoMapping = MappingDtos(ref groupDtos, fieldMapping, keyColumns);
            var hasError = false;
            string getError(string key) => _lang[$"BulkUploadCommand.{key}"];

            foreach (var map in dtoMapping)
            {
                var dto = map.Key;
                var row = map.Value;

                if (row is null) continue;

                var errors = new List<string>();

                if (string.IsNullOrEmpty(dto.Name))
                    errors.Add(getError("GroupNameRequired.Message"));

                else if (groups.Any(x => x.Id != dto.Id && x.Name!.Equals(dto.Name, StringComparison.InvariantCultureIgnoreCase)) ||
                    _dbContext.ItemGroups.GetAll(_actor).Any(x => x.Name == dto.Name && x.Id != dto.Id))
                    errors.Add(getError("GroupNameAlreadyExist.Message"));

                row.Errors = errors.Count == 0 ? null : string.Join(", ", errors);
                if (errors.Count > 0) hasError = true;
            }

            return !hasError ? Result.OK() : Result.Failed(getError("ExcelRowError.Message"));
        }



        void MappingGroup(ItemGroup group, ItemGroupBulkUploadDetail row, Dictionary<string, string> fieldMapping, List<string> keyColumns)
        {
            if (fieldMapping.Count == 0) return;

            var setters = new Dictionary<string, Action<string?>>()
            {
                { Fields.Name, (name) => group.Name = name }
            };

            foreach (var key in fieldMapping!.Select(x => x.Key))
            {
                if (keyColumns.Contains(key)) continue;

                var rowColumn = fieldMapping![key];
                var getter = ExcelFieldConverters[rowColumn];
                var setter = setters[key];
                setter(getter(row));
            }
        }

        List<KeyValuePair<ItemGroupDto, ItemGroupBulkUploadDetail?>> MappingDtos(ref List<ItemGroupDto> groups, Dictionary<string, string> fieldMapping, List<string> keyColumns)
        {
            var rows = GetExcelRows();
            var result = new List<KeyValuePair<ItemGroupDto, ItemGroupBulkUploadDetail?>>();

            foreach (var group in groups)
            {
                var row = GetRowByGroup(group, rows, fieldMapping, keyColumns);
                if (row is null) continue;

                result.Add(new(group, row));
                MappingGroupDto(group, row, fieldMapping, keyColumns);
            }

            return result;
        }

        void MappingGroupDto(ItemGroupDto group, ItemGroupBulkUploadDetail row, Dictionary<string, string> fieldMapping, List<string> keyColumns)
        {
            if (fieldMapping.Count == 0) return;

            var setters = new Dictionary<string, Action<string?>>()
            {
                { Fields.Name, (name) => group.Name = name }
            };

            foreach (var key in fieldMapping!.Select(x => x.Key))
            {
                if (keyColumns.Contains(key)) continue;

                var rowColumn = fieldMapping[key];
                var getter = ExcelFieldConverters[rowColumn];
                var setter = setters[key];
                setter(getter(row));
            }
        }
    }
}
