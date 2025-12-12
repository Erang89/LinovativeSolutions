using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Shared.Dto.ItemDtos;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group
{
    public abstract class BulkOperationGroupBase(IAppDbContext dbContext, IActor actor, ILangueageService lang, CrudOperations operation) :
        BulkOperationProcesBase<ItemGroupBulkUpload, ItemGroupBulkUploadDetail>(dbContext, actor, lang, operation)
    {

        protected static class Keys
        {
            public const string Id = nameof(ItemGroupViewDto.Id);
            public const string Name = nameof(ItemGroupViewDto.Name);
        }
        private static class Columns
        {
            public const string Column1 = nameof(ItemGroupBulkUploadDetail.Column1);
            public const string Column2 = nameof(ItemGroupBulkUploadDetail.Column2);
        }


        protected override string EntityKey => "Group";
        protected override IDictionary<string, Func<string?, object?>> Converters => new Dictionary<string, Func<string?, object?>>()
        {
            {Keys.Id, static (x) => StringToGuid(x)},
            {Keys.Name, static (x) => x}
        };

        protected override IDictionary<string, Func<ItemGroupBulkUploadDetail, string?>> Getters => new Dictionary<string, Func<ItemGroupBulkUploadDetail, string?>>()
        {
            {Columns.Column1, static (x) => x.Column1},
            {Columns.Column2, static (x) => x.Column2},
        };

        protected override IQueryable<ItemGroupBulkUploadDetail> FilterRecords(IQueryable<ItemGroupBulkUploadDetail> query, Guid? uploadId)
            => query.Where(x => x.ItemGroupBulkUploadId == uploadId);


        protected override string? GetExcelHeader(string key)
        {
            var upload = GetBulkUpload()!;
            return key switch
            {
                Columns.Column1 => upload.headerColum1,
                Columns.Column2 => upload.headerColum2,
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
                    _ => () => true
                };

                if (!func()) anyError = true;
            }

            return !anyError;
        }


        protected override bool IdsShouldExist(IEnumerable<Guid> ids)
        {
            var groupIds = _dbContext.ItemGroups.Where(x => ids.Contains(x.Id)).Select(x => x.Id);
            var notFoundIds = groupIds.Where(x => !ids.Contains(x)).ToList();
            var (cell, converter) = GetGetterAndConverter(_columnId);
            var notFoundIdsString = notFoundIds.Select(x => x.ToString()).ToList();
            foreach (var row in GetRecords().Where(x => notFoundIdsString.Contains(converter(cell(x)!))))
                row.AddError(GetError("ValueNotFoundInTheSystem.Message", cell(row)));

            return !notFoundIds.Any();
        }


        protected override bool IdsShouldNOTExist(IEnumerable<Guid> ids)
        {
            var existingIds = _dbContext.ItemGroups.Where(x => ids.Contains(x.Id)).Select(x => x.Id).ToList();
            var existingIdsString = existingIds.Select(x => x.ToString()).ToList();
            var (cell, _) = GetGetterAndConverter(_columnId);
            foreach (var row in GetRecords().Where(x => existingIdsString.Contains(cell(x)!)))
                row.AddError(GetError("ValueAlreadyExistInTheSystem.Message", cell(row)));

            return !existingIds.Any();
        }


        // Private Functions

        bool ValidateName()
        {
            if (_operation == CrudOperations.Delete) return true;

            var validEmptyCheck = CheckEmptyName();
            var validDuplicateCheck = CheckDuplicateName();
            return validEmptyCheck && validDuplicateCheck;
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
            var predicateDuplicateName = _dbContext.ItemGroups.GetAll(_actor).Where(x => !ids.Contains(x.Id) && names.Contains(x.Name)).Select(x => x.Name!.ToLower()).ToList();
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
