using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings
{
    internal class BulkMappingGroupUpdateService : BulkMappingGroupBase, IBulkMapping
    {

        private readonly ILangueageService _lang;

        public BulkMappingGroupUpdateService(ILangueageService lang, IAppDbContext dbContext, IActor actor) : base(lang, dbContext, actor, CrudOperations.Update)
        {
            _lang = lang;
            _lang.EnsureLoad(x => x.BulkUploadCommand);
        }

        public async Task<Result> Save(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token)
        {
            var validate = await Validate(fieldMapping, keyColumns, token);

            if (!validate)
            {
                await _dbContext.SaveAsync(_actor);
                return validate;
            }

            var groups = await GetGroups(fieldMapping, keyColumns);
            var rows =  GetExcelRows();


            foreach (var group in groups)
            {
                var row = GetRowByGroup(group, rows, fieldMapping, keyColumns);
                if (row is null) continue;

                Mapping(group, row, fieldMapping, keyColumns);
            }

            await _dbContext.SaveAsync(_actor);
            return Result.OK();
        }


        public override async Task<Result> Validate(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token)
        {

            var validate =  await base.Validate(fieldMapping, keyColumns, token);
            if (!validate) return validate;

            var groups = await GetGroups(fieldMapping, keyColumns);
            var groupDtos = groups.Select(x =>  new ItemGroupDto() { Id = x.Id, Name = x.Name}).ToList();
            var dtoMapping = MappingDtos(ref groupDtos, fieldMapping, keyColumns);
            var hasError = false;
            Func<string, string> getError = (key) => _lang[$"BulkUploadCommand.{key}"];

            foreach (var map in dtoMapping)
            {
                var dto = map.Key;
                var row = map.Value;

                if(row is null) continue;

                var errors = new List<string>();
               
                if (string.IsNullOrEmpty(dto.Name))
                    errors.Add(getError("GroupNameRequired.Message"));

                if(groups.Any(x => x.Id != dto.Id && x.Name.Equals(dto.Name, StringComparison.InvariantCultureIgnoreCase)) ||
                    _dbContext.ItemGroups.GetAll(_actor).Any(x => x.Name == dto.Name && x.Id != dto.Id))
                    errors.Add(getError("GroupNameAlreadyExist.Message"));

                row.Errors = errors.Count == 0 ? null : string.Join(", ", errors);
                if(errors.Count > 0) hasError = true;   
            }

            return !hasError?  Result.OK() : Result.Failed(getError("ExcelRowError.Message"));
        }



        void Mapping(ItemGroup group, ItemGroupBulkUploadDetail row, Dictionary<string, string> fieldMapping, List<string> keyColumns)
        {
            if(fieldMapping.Count == 0) return;

            var setters = new Dictionary<string, Action<string?>>()
            {
                { Fields.Name, (name) => group.Name = name }
            };

            foreach (var field in fieldMapping!)
            {
                if (keyColumns.Contains(field.Key)) continue;

                var rowColumn = fieldMapping[field.Key];
                var getter = ExcelFieldConverters[rowColumn];
                var setter = setters[field.Key];
                setter(getter(row));
            }
        }

        List<KeyValuePair<ItemGroupDto, ItemGroupBulkUploadDetail?>> MappingDtos(ref List<ItemGroupDto> groups,  Dictionary<string, string> fieldMapping, List<string> keyColumns)
        {
            var rows = GetExcelRows();
            var result = new List<KeyValuePair<ItemGroupDto, ItemGroupBulkUploadDetail?>>();

            foreach (var group in groups)
            {
                var row = GetRowByGroup(group, rows, fieldMapping, keyColumns);
                if (row is null) continue;

                result.Add(new (group, row));
                Mapping(group, row, fieldMapping, keyColumns);
            }

            return result;
        }
        
        void Mapping(ItemGroupDto group, ItemGroupBulkUploadDetail row, Dictionary<string, string> fieldMapping, List<string> keyColumns)
        {
            if (fieldMapping.Count == 0) return;

            var setters = new Dictionary<string, Action<string?>>()
            {
                { Fields.Name, (name) => group.Name = name }
            };

            foreach (var field in fieldMapping!)
            {
                if (keyColumns.Contains(field.Key)) continue;

                var rowColumn = fieldMapping[field.Key];
                var getter = ExcelFieldConverters[rowColumn];
                var setter = setters[field.Key];
                setter(getter(row));
            }
        }
    }
}
