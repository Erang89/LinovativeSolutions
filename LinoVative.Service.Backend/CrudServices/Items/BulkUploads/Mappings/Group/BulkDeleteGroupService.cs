using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Shared.Dto;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group
{
    public class BulkDeleteGroupService : BulkOperationItemGroupBase, IBulkMapping
    {
        public BulkDeleteGroupService(ILangueageService lang, IAppDbContext dbContext, IActor actor) : base(lang, dbContext, actor, CrudOperations.Delete)
        {

        }

        public async Task<Result> Save(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token)
        {
            var validate = await Validate(fieldMapping, keyColumns, token);
            if (!validate) return validate;

            var groups = await GetGroups(fieldMapping, keyColumns);
            var excelRows = GetExcelRows();
            var selectGroups = groups.Where(_ => true);

            foreach(var key in keyColumns)
            {
                var excelField = fieldMapping[key];
                var cellGeter = ExcelFieldConverters[excelField];
                var converter = GroupFieldConverters[key];
                var inputValues = excelRows.Select(x => converter(cellGeter(x))).ToList();

                selectGroups = key switch
                {
                    Fields.Id => selectGroups.Where(x => inputValues.Contains(x.Id)),
                    Fields.Name => selectGroups.Where(x => inputValues.Contains(x.Name)),
                    _ => selectGroups
                };
            }

            foreach (var group in selectGroups)
                group.Delete(_actor);

            await _dbContext.SaveAsync(_actor);
            await DeleteBulkUploadRecords();
            return Result.OK();
        }


        public override async Task<Result> Validate(Dictionary<string, string> fieldMapping, List<string> keyColumns, CancellationToken token)
        {
            var result = await base.Validate(fieldMapping, keyColumns, token);
            if (!result) return result;

            if (keyColumns.Count == 0) return Result.Failed(GetError("NoKeyColumns.Message", null));

            return Result.OK();
        }
    }
}
