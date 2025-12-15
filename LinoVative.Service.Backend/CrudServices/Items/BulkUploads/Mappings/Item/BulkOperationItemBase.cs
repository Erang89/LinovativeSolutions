using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Shared.Dto.ItemDtos;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Item
{
    public abstract class BulkOperationItemBase(IAppDbContext dbContext, IActor actor, ILangueageService lang, CrudOperations operation) :
        BulkOperationProcesBase<ItemBulkUpload, ItemBulkUploadDetail>(dbContext, actor, lang, operation)
    {

        const string RequiredMessageResourceKey = "ExcelFieldValueRequires.Message";
        const string AlreadyExistMessageResourceKey = "ValueAlreadyExistInTheSystem.Message";
        const string ResourceMessage = "BulkUploadCommand";

        protected static class Keys
        {
            public const string Id = nameof(ItemViewDto.Id);
            public const string Name = nameof(ItemViewDto.Name);
            public const string Unit = nameof(ItemViewDto.Unit);
            public const string Category = nameof(ItemViewDto.Category);
            public const string Description = nameof(ItemViewDto.Description);
            public const string IsActive = nameof(ItemViewDto.IsActive);
            public const string SellPrice = nameof(ItemViewDto.SellPrice);
            public const string Code = nameof(ItemViewDto.Code);
        }

        private static class Columns
        {
            public const string Column1 = nameof(ItemBulkUploadDetail.Column1);
            public const string Column2 = nameof(ItemBulkUploadDetail.Column2);
            public const string Column3 = nameof(ItemBulkUploadDetail.Column3);
            public const string Column4 = nameof(ItemBulkUploadDetail.Column4);
            public const string Column5 = nameof(ItemBulkUploadDetail.Column5);
            public const string Column6 = nameof(ItemBulkUploadDetail.Column6);
            public const string Column7 = nameof(ItemBulkUploadDetail.Column7);
        }


        protected override string EntityKey => "Item";
        protected override IDictionary<string, Func<string?, object?>> Converters => new Dictionary<string, Func<string?, object?>>()
        {
            {Keys.Id, static (x) => StringToGuid(x)},
            {Keys.Name, static (x) => x},
            {Keys.Unit, static (x) => x},
            {Keys.Category, static (x) => x},
            {Keys.Description, static (x) => x},
            {Keys.IsActive, static (x) => bool.TryParse(x, out bool result)? result : null},
            {Keys.SellPrice, static (x) => decimal.TryParse(x, out decimal result)? result : null},
            {Keys.Code, static (x) => x},
        };

        protected override IDictionary<string, Func<ItemBulkUploadDetail, string?>> Getters => new Dictionary<string, Func<ItemBulkUploadDetail, string?>>()
        {
            {Columns.Column1, static (x) => x.Column1},
            {Columns.Column2, static (x) => x.Column2},
            {Columns.Column3, static (x) => x.Column3},
            {Columns.Column4, static (x) => x.Column4},
            {Columns.Column5, static (x) => x.Column5},
            {Columns.Column6, static (x) => x.Column6},
            {Columns.Column7, static (x) => x.Column7},
        };

        protected override IQueryable<ItemBulkUploadDetail> FilterRecords(IQueryable<ItemBulkUploadDetail> query, Guid? uploadId)
            => query.Where(x => x.ItemBulkUploadId == uploadId);


        protected override string? GetExcelHeader(string key)
        {
            var upload = GetBulkUpload()!;
            return key switch
            {
                Columns.Column1 => upload.headerColum1,
                Columns.Column2 => upload.headerColum2,
                Columns.Column3 => upload.headerColum3,
                Columns.Column4 => upload.headerColum4,
                Columns.Column5 => upload.headerColum5,
                Columns.Column6 => upload.headerColum6,
                Columns.Column7 => upload.headerColum7,
                _ => null
            };
        }


        protected override bool IsValidFields()
        {
            var anyError = false;

            foreach (var key in _fieldMapping.Keys)
            {
                Func<bool> func = key switch
                {
                    Keys.Name => ValidateName,
                    Keys.Code => ValidateCode,
                    Keys.Unit => ValidateUnit,
                    Keys.Category => ValidateCategory,
                    Keys.IsActive => ValidateIsActive,
                    Keys.SellPrice => ValidatePrice,
                    _ => () => true
                };

                if (!func()) anyError = true;
            }

            return !anyError;
        }


        protected override bool IdsShouldExist(IEnumerable<Guid> ids)
        {
            var query = _dbContext.Items.Where(x => ids.Contains(x.Id));
            if (_operation is CrudOperations.Delete or CrudOperations.Update) query = query.GetAll(_actor);

            var groupIds = query.Select(x => x.Id);
            var notFoundIds = ids.Where(x => !groupIds.Contains(x)).ToList();
            var (cell, _) = GetGetterAndConverter(_columnId);
            var notFoundIdsString = notFoundIds.Select(x => x.ToString()).ToList();
            foreach (var row in GetRecords().Where(x => notFoundIdsString.Contains(cell(x)!)))
                row.AddError(GetError("ValueNotFoundInTheSystem.Message", cell(row)));

            return notFoundIds.Count == 0;
        }


        protected override bool IdsShouldNOTExist(IEnumerable<Guid> ids)
        {
            var existingIds = _dbContext.Items.Where(x => ids.Contains(x.Id)).Select(x => x.Id).ToList();
            var existingIdsString = existingIds.Select(x => x.ToString()).ToList();
            var (cell, _) = GetGetterAndConverter(_columnId);
            foreach (var row in GetRecords().Where(x => existingIdsString.Contains(cell(x)!)))
                row.AddError(GetError(AlreadyExistMessageResourceKey, cell(row)));

            return existingIds.Count == 0;
        }


        // Private Functions

        // Validate Price
        private bool ValidatePrice()
        {
            var rows = GetRecords();
            var (cell, _) = GetGetterAndConverter(Keys.SellPrice);
            var excelColumn = _fieldMapping[Keys.SellPrice];
            var columnName = GetExcelHeader(excelColumn);
            var nullValues = rows.Where(x => string.IsNullOrWhiteSpace(cell(x))).ToList();
            var invalidValues = rows.Where(x => !nullValues.Contains(x) && !decimal.TryParse(cell(x), out _)).ToList();
            var invalidRangeValue = rows.Where(x => !nullValues.Contains(x) && !invalidValues.Contains(x) && decimal.TryParse(cell(x), out decimal price) && price < 0).ToList();
            foreach (var row in nullValues) row.AddError(GetError(RequiredMessageResourceKey, columnName));
            foreach (var row in invalidValues) row.AddError(GetError("FailedToConvertAsDecimal.Message", cell(row)));
            foreach (var row in invalidRangeValue) row.AddError(string.Format(_lang[$"{ResourceMessage}.ValueMustGreaterOrEqualThan.Message"], columnName, "0"));
            return nullValues.Count == 0 && invalidValues.Count == 0 && invalidRangeValue.Count == 0;
        }


        // Validate Is Active
        private bool ValidateIsActive()
        {
            if (_operation == CrudOperations.Delete) return true;

            var (cell, _) = GetGetterAndConverter(Keys.IsActive);
            var rows = GetRecords();
            var invalidRows = rows.Where(x => !string.IsNullOrWhiteSpace(cell(x)) && cell(x)!.ToLower() is not "true" or "false").ToList();
            foreach (var row in invalidRows)
            {
                row.AddError(GetError("FailedToConvertAsBoolean.Message", cell(row)));
            }

            return invalidRows.Count == 0;
        }



        // Validate Category
        private bool ValidateCategory()
        {
            if (_operation == CrudOperations.Delete) return true;

            var validateExist = CheckCategorytExist();
            var validateEmpty = CheckEmtyCategory();
            return validateExist && validateEmpty;

        }

        private bool CheckCategorytExist()
        {
            var (cell, _) = GetGetterAndConverter(Keys.Category);
            var rows = GetRecords();
            var inputNames = GetInputValues(Keys.Category).Select(x => (string)x!).ToList();
            var existingIdNames = _dbContext.ItemCategories.GetAll(_actor).Select(x => x.Name);
            var notExistNames = inputNames.Where(x => !existingIdNames.Contains(x));
            var invalidRows = rows.Where(x => notExistNames.Contains(cell(x)));
            foreach (var row in invalidRows)
            {
                row.AddError(GetError("ValueNotFoundInTheSystem.Message", cell(row)));
            }
            return !invalidRows.Any();
        }

        private bool CheckEmtyCategory()
        {
            var (cell, _) = GetGetterAndConverter(Keys.Category);
            var isAnyError = false;
            var rows = GetRecords();
            var columnKey = _fieldMapping[Keys.Category];
            var headerName = GetExcelHeader(columnKey);

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError(RequiredMessageResourceKey, headerName));
                isAnyError = true;
            }

            return !isAnyError;
        }


        // Validate Unit
        private bool ValidateUnit()
        {
            if (_operation == CrudOperations.Delete) return true;

            var validateExist = CheckUnitExist();
            var validateEmpty = CheckEmtyUnit();
            return validateExist && validateEmpty;

        }

        private bool CheckUnitExist()
        {
            var (cell, _) = GetGetterAndConverter(Keys.Unit);
            var rows = GetRecords();
            var inputNames = GetInputValues(Keys.Unit).Select(x => (string)x!).ToList();
            var existingIdNames = _dbContext.ItemUnits.GetAll(_actor).Select(x => x.Name);
            var notExistNames = inputNames.Where(x => !existingIdNames.Contains(x));
            var invalidRows = rows.Where(x => notExistNames.Contains(cell(x)));
            foreach (var row in invalidRows)
            {
                row.AddError(GetError("ValueNotFoundInTheSystem.Message", cell(row)));
            }
            return !invalidRows.Any();
        }

        private bool CheckEmtyUnit()
        {
            var (cell, _) = GetGetterAndConverter(Keys.Unit);
            var isAnyError = false;
            var rows = GetRecords();
            var columnKey = _fieldMapping[Keys.Unit];
            var headerName = GetExcelHeader(columnKey);

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError(RequiredMessageResourceKey, headerName));
                isAnyError = true;
            }

            return !isAnyError;
        }



        // Validate Code
        private bool ValidateCode()
        {
            if (_operation == CrudOperations.Delete) return true;

            var validEmptyCheck = CheckEmptyCode();
            var validDuplicateCheck = CheckDulicateCode();
            return validEmptyCheck && validDuplicateCheck;

        }


        bool CheckEmptyCode()
        {
            var (cell, _) = GetGetterAndConverter(Keys.Code);
            var isAnyError = false;
            var rows = GetRecords();
            var columnKey = _fieldMapping[Keys.Code];
            var headerName = GetExcelHeader(columnKey);

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError(RequiredMessageResourceKey, headerName));
                isAnyError = true;
            }

            return !isAnyError;
        }


        bool CheckDulicateCode()
        {
            var (idCell, idConverter) = GetGetterAndConverter(Keys.Id);
            var (nameCell, _) = GetGetterAndConverter(Keys.Code);
            var rows = GetRecords();
            var rowMapping = rows.Select(x => new { Id = idConverter(idCell(x) ?? Guid.NewGuid().ToString()), Code = nameCell(x), Row = x }).Where(x => !string.IsNullOrWhiteSpace(x.Code)).ToList();
            var ids = rowMapping.Select(x => x.Id).ToList();
            var codes = rowMapping.Select(x => x.Code).ToList();
            var predicateDuplicateCode = _dbContext.Items.GetAll(_actor).Where(x => !ids.Contains(x.Id) && codes.Contains(x.Code)).Select(x => x.Code!.ToLower()).ToList();
            var invalidRows = rowMapping.Where(x => predicateDuplicateCode.Contains(x.Code!.ToLower()));
            foreach (var row in invalidRows)
            {
                row.Row.AddError(GetError(AlreadyExistMessageResourceKey, row.Code));
            }
            return !invalidRows.Any();
        }


        // Validate Name
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

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError(RequiredMessageResourceKey, headerName));
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
            var predicateDuplicateName = _dbContext.Items.GetAll(_actor).Where(x => !ids.Contains(x.Id) && names.Contains(x.Name)).Select(x => x.Name!.ToLower()).ToList();
            var invalidRows = rowMapping.Where(x => predicateDuplicateName.Contains(x.Name!.ToLower()));
            foreach (var row in invalidRows)
            {
                row.Row.AddError(GetError(AlreadyExistMessageResourceKey, row.Name));
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
