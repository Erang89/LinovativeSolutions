using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Shared.Dto.ItemDtos;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Category
{
    public abstract class BulkOperationCategoryBase(IAppDbContext dbContext, IActor actor, ILangueageService lang, CrudOperations operation) :
        BulkOperationProcesBase<ItemCategoryBulkUpload, ItemCategoryBulkUploadDetail>(dbContext, actor, lang, operation)
    {

        protected static class Keys
        {
            public const string Id = nameof(ItemCategoryViewDto.Id);
            public const string Name = nameof(ItemCategoryViewDto.Name);
            public const string Group = nameof(Group);
        } 
        private static class Columns
        {
            public const string Column1 = nameof(ItemCategoryBulkUploadDetail.Column1);
            public const string Column2 = nameof(ItemCategoryBulkUploadDetail.Column2);
            public const string Column3 = nameof(ItemCategoryBulkUploadDetail.Column3);
        }


        protected override string EntityKey => "Category";
        protected override IDictionary<string, Func<string?, object?>> Converters => new Dictionary<string, Func<string?, object?>>()
        {
            {Keys.Id, static (x) => StringToGuid(x)},
            {Keys.Name, static (x) => x},
            {Keys.Group, static (x) => x}
        };

        protected override IDictionary<string, Func<ItemCategoryBulkUploadDetail, string?>> Getters => new Dictionary<string, Func<ItemCategoryBulkUploadDetail, string?>>()
        {
            {Columns.Column1, static (x) => x.Column1},
            {Columns.Column2, static (x) => x.Column2},
            {Columns.Column3, static (x) => x.Column3},
        };

        protected override IQueryable<ItemCategoryBulkUploadDetail> FilterRecords(IQueryable<ItemCategoryBulkUploadDetail> query, Guid? uploadId)
            => query.Where(x => x.ItemCategoryBulkUploadId == uploadId);


        protected override string? GetExcelHeader(string key)
        {
            var upload = GetBulkUpload()!;
            return key switch
            {
                Columns.Column1 => upload.headerColum1,
                Columns.Column2 => upload.headerColum2,
                Columns.Column3 => upload.headerColum3,
                _ => null
            };
        }


        protected override bool IsValidFields()
        {
            var anyError = false;

            foreach(var key in _fieldMapping.Keys)
            {
                Func<bool> func = key switch
                {
                    Keys.Name => ValidateName,
                    Keys.Group => ValidateGroup,
                    _ => () => true
                };

                if (!func()) anyError = true;
            }

            return !anyError;
        }

        protected override bool IdsShouldExist(IEnumerable<Guid> ids)
        {
            var groupIds = _dbContext.ItemCategories.GetAll(_actor).Where(x => ids.Contains(x.Id)).Select(x => x.Id);
            var notFoundIds = ids.Where(x => !groupIds.Contains(x)).ToList();
            var (cell, _) = GetGetterAndConverter(_columnId);
            var notFoundIdsString = notFoundIds.Select(x => x.ToString()).ToList();
            foreach (var row in GetRecords().Where(x => notFoundIdsString.Contains(cell(x)!)))
                row.AddError(GetError("ValueNotFoundInTheSystem.Message", cell(row)));

            return notFoundIds.Count == 0;
        }


        protected override bool IdsShouldNOTExist(IEnumerable<Guid> ids)
        {
            var existingIds = _dbContext.ItemCategories.Where(x => ids.Contains(x.Id)).Select(x => x.Id).ToList();
            var existingIdsString = existingIds.Select(x => x.ToString()).ToList();
            var (cell, _) = GetGetterAndConverter(_columnId);
            foreach (var row in GetRecords().Where(x => existingIdsString.Contains(cell(x)!)))
                row.AddError(GetError("ValueAlreadyExistInTheSystem.Message", cell(row)));

            return existingIds.Count == 0;
        }


        // Private Functions

        bool ValidateName()
        {
            if (_operation == CrudOperations.Delete) return true;

            var validEmptyCheck = CheckEmptyName();
            var validDuplicateCheck = CheckDuplicateName();
            return validEmptyCheck && validDuplicateCheck;
        }


        private bool ValidateGroup()
        {
            if(_operation == CrudOperations.Delete) return true;

            var validateEmptyGroup = CheckEmtpyGroup();
            var validateExisistGroup = CheckGroupExist();

            return validateEmptyGroup && validateExisistGroup;
        }

        private bool CheckGroupExist()
        {
            var (groupCell, _) = GetGetterAndConverter(Keys.Group);
            var rows = GetRecords();
            var inputNames = GetInputValues(Keys.Group).Select(x => (string)x!).Where(x => !string.IsNullOrEmpty(x)).ToList();
            var existingIdNames = _dbContext.ItemGroups.GetAll(_actor).Select(x => x.Name);
            var notExistNames = inputNames.Where(x => !existingIdNames.Contains(x));
            var invalidRows = rows.Where(x => notExistNames.Contains(groupCell(x)));
            foreach (var row in invalidRows)
            {
                row.AddError(GetError("FieldNotExistInTheSystem.Message", groupCell(row)));
            }
            return !invalidRows.Any();
        }

        private bool CheckEmtpyGroup()
        {
            var (cell, _) = GetGetterAndConverter(Keys.Group);
            var isAnyError = false;
            var rows = GetRecords();
            var columnKey = _fieldMapping[Keys.Group];
            var headerName = GetExcelHeader(columnKey);

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError("ExcelFieldValueRequires.Message", headerName));
                isAnyError = true;
            }

            return !isAnyError;
        }

        bool CheckEmptyName()
        {
            var (cell, _) = GetGetterAndConverter(Keys.Name);
            var isAnyError = false;
            var rows = GetRecords();
            var columnKey = _fieldMapping[Keys.Name];
            var headerName = GetExcelHeader(columnKey);

            foreach(var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError("ExcelFieldValueRequires.Message", headerName));
                isAnyError = true;
            }

            return !isAnyError;
        }

        bool CheckDuplicateName()
        {
            var (idCell, idConverter) = GetGetterAndConverter(_columnId);
            var (nameCell, _) = GetGetterAndConverter(Keys.Name);
            var rows = GetRecords();
            var rowMapping = rows.Select(x => new { Id = idConverter(idCell(x) ?? Guid.NewGuid().ToString()), Name = nameCell(x), Row = x }).Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToList();
            var ids = rowMapping.Select(x => x.Id).ToList();
            var names = rowMapping.Select(x => x.Name).ToList();
            var predicateDuplicateName = _dbContext.ItemCategories.GetAll(_actor).Where(x => !ids.Contains(x.Id) && names.Contains(x.Name)).Select(x => x.Name!.ToLower()).ToList();
            var invalidRows = rowMapping.Where(x => predicateDuplicateName.Contains(x.Name!.ToLower()));
            foreach (var row in invalidRows)
            {
                row.Row.AddError(GetError("ValueAlreadyExistInTheSystem.Message", row.Name));
            }
            return !invalidRows.Any();
        }

        static Guid? StringToGuid(string? str)
        {
            if (str == null) return null;
            if (Guid.TryParse(str, out var id)) return id;
            return null;
        }
    }
}
