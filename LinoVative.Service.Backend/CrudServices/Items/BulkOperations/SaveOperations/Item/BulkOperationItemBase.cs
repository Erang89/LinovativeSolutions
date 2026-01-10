using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Shared.Dto.ItemDtos;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Item
{
    public abstract class BulkOperationItemBase(IAppDbContext dbContext, IActor actor, ILangueageService lang, CrudOperations operation) :
        BulkOperationProcesBase<ItemBulkUpload, ItemBulkUploadDetail>(dbContext, actor, lang, operation)
    {

        const string RequiredMessageResourceKey = "ExcelFieldValueRequires.Message";
        const string AlreadyExistMessageResourceKey = "ValueAlreadyExistInTheSystem.Message";
        const string ResourceMessage = "BulkUploadCommand";

        protected static class Keys
        {
            public const string Id = nameof(SKUItemViewDto.Id);
            public const string ItemName = nameof(SKUItemViewDto.ItemName);
            public const string CategoryName = nameof(SKUItemViewDto.CategoryName);
            public const string SKU = nameof(SKUItemViewDto.SKU);
            public const string VarianName = nameof(SKUItemViewDto.VarianName);
            public const string UnitName = nameof(SKUItemViewDto.UnitName);
            public const string IsActive = nameof(SKUItemViewDto.IsActive);
            public const string IsPurchaseItem = nameof(SKUItemViewDto.IsPurchaseItem);
            public const string IsSaleItem = nameof(SKUItemViewDto.IsSaleItem);
            public const string SalePrice = nameof(SKUItemViewDto.SalePrice);
            public const string DefaultPurchaseQty = nameof(SKUItemViewDto.DefaultPurchaseQty);
            public const string MinimumStockQty = nameof(SKUItemViewDto.MinimumStockQty);
            public const string ItemDescription = nameof(SKUItemViewDto.ItemDescription);
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
            public const string Column8 = nameof(ItemBulkUploadDetail.Column8);
            public const string Column9 = nameof(ItemBulkUploadDetail.Column9);
            public const string Column10 = nameof(ItemBulkUploadDetail.Column10);
            public const string Column11 = nameof(ItemBulkUploadDetail.Column11);
            public const string Column12 = nameof(ItemBulkUploadDetail.Column12);
            public const string Column13 = nameof(ItemBulkUploadDetail.Column13);
        }


        protected override string EntityKey => "Item";
        protected override IDictionary<string, Func<string?, object?>> Converters => new Dictionary<string, Func<string?, object?>>()
        {
            {Keys.Id, static (x) => StringToGuid(x)},
            {Keys.ItemName, static (x) => x},
            {Keys.CategoryName, static (x) => x},
            {Keys.SKU, static (x) => x},
            {Keys.VarianName, static (x) => x},
            {Keys.UnitName, static (x) => x},
            {Keys.IsActive, static (x) => bool.TryParse(x, out bool result)? result : null},
            {Keys.IsPurchaseItem, static (x) => bool.TryParse(x, out bool result)? result : null},
            {Keys.IsSaleItem, static (x) => bool.TryParse(x, out bool result)? result : null},
            {Keys.SalePrice, static (x) => decimal.TryParse(x, out decimal result)? result : null},
            {Keys.DefaultPurchaseQty, static (x) => decimal.TryParse(x, out decimal result)? result : null},
            {Keys.MinimumStockQty, static (x) => decimal.TryParse(x, out decimal result)? result : null},
            {Keys.ItemDescription, static (x) => x}
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
            {Columns.Column8, static (x) => x.Column8},
            {Columns.Column9, static (x) => x.Column9},
            {Columns.Column10, static (x) => x.Column10},
            {Columns.Column11, static (x) => x.Column11},
            {Columns.Column12, static (x) => x.Column12},
            {Columns.Column13, static (x) => x.Column13},
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
                Columns.Column8 => upload.headerColum8,
                Columns.Column9 => upload.headerColum9,
                Columns.Column10 => upload.headerColum10,
                Columns.Column11 => upload.headerColum11,
                Columns.Column12 => upload.headerColum12,
                Columns.Column13 => upload.headerColum13,
                _ => null
            };
        }



        #region Override Checkeing
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
        #endregion


        protected override bool IsValidFields()
        {
            var anyError = false;

            foreach (var key in _fieldMapping.Keys)
            {
                Func<bool> func = key switch
                {
                    Keys.ItemName => ValidateItemName,
                    Keys.CategoryName => ValidateCategory,
                    Keys.SKU => ValidateSKU,
                    Keys.VarianName => ValidateVariantName,
                    Keys.UnitName => ValidateUnit,
                    Keys.IsActive => ValidateIsActive,
                    Keys.IsPurchaseItem => ValidateIsPurchaseItem,
                    Keys.IsSaleItem => ValidateIsSalesItem,
                    Keys.SalePrice => ValidatePrice,
                    Keys.DefaultPurchaseQty => ValidatePurchaseQty,
                    Keys.MinimumStockQty => ValidateMinimumStockQty,
                    _ => () => true
                };

                if (!func()) anyError = true;
            }

            return !anyError;
        }



        // Private Functions

        #region Validate Price
        private bool ValidatePrice()
        {
            var rows = GetRecords();
            var (cell, _) = GetGetterAndConverter(Keys.SalePrice);
            var excelColumn = _fieldMapping[Keys.SalePrice];
            var columnName = GetExcelHeader(excelColumn);
            var nullValues = rows.Where(x => string.IsNullOrWhiteSpace(cell(x))).ToList();
            var invalidValues = rows.Where(x => !nullValues.Contains(x) && !decimal.TryParse(cell(x), out _)).ToList();
            var invalidRangeValue = rows.Where(x => !nullValues.Contains(x) && !invalidValues.Contains(x) && decimal.TryParse(cell(x), out decimal price) && price < 0).ToList();
            foreach (var row in nullValues) row.AddError(GetError(RequiredMessageResourceKey, columnName));
            foreach (var row in invalidValues) row.AddError(GetError("FailedToConvertAsDecimal.Message", cell(row)));
            foreach (var row in invalidRangeValue) row.AddError(string.Format(_lang[$"{ResourceMessage}.ValueMustGreaterOrEqualThan.Message"], columnName, "0"));
            return nullValues.Count == 0 && invalidValues.Count == 0 && invalidRangeValue.Count == 0;
        }
        #endregion
        

        #region Validate Purchase Qty
        private bool ValidatePurchaseQty()
        {
            var rows = GetRecords();
            var (cell, _) = GetGetterAndConverter(Keys.DefaultPurchaseQty);
            var excelColumn = _fieldMapping[Keys.DefaultPurchaseQty];
            var columnName = GetExcelHeader(excelColumn);
            var invalidValues = rows.Where(x => !string.IsNullOrWhiteSpace(cell(x)) && !decimal.TryParse(cell(x), out _)).ToList();
            var invalidRangeValue = rows.Where(x => !string.IsNullOrWhiteSpace(cell(x)) && decimal.TryParse(cell(x), out decimal price) && price < 0).ToList();
            foreach (var row in invalidValues) row.AddError(GetError("FailedToConvertAsDecimal.Message", cell(row)));
            foreach (var row in invalidRangeValue) row.AddError(string.Format(_lang[$"{ResourceMessage}.ValueMustGreaterOrEqualThan.Message"], columnName, "0"));
            return invalidValues.Count == 0 && invalidRangeValue.Count == 0;
        }
        #endregion


        #region Validate Minimum Stock Qty
        private bool ValidateMinimumStockQty()
        {
            var rows = GetRecords();
            var (cell, _) = GetGetterAndConverter(Keys.MinimumStockQty);
            var excelColumn = _fieldMapping[Keys.MinimumStockQty];
            var columnName = GetExcelHeader(excelColumn);
            var invalidValues = rows.Where(x => !string.IsNullOrWhiteSpace(cell(x)) && !decimal.TryParse(cell(x), out _)).ToList();
            var invalidRangeValue = rows.Where(x => !string.IsNullOrWhiteSpace(cell(x)) && decimal.TryParse(cell(x), out decimal price) && price < 0).ToList();
            foreach (var row in invalidValues) row.AddError(GetError("FailedToConvertAsDecimal.Message", cell(row)));
            foreach (var row in invalidRangeValue) row.AddError(string.Format(_lang[$"{ResourceMessage}.ValueMustGreaterOrEqualThan.Message"], columnName, "0"));
            return invalidValues.Count == 0 && invalidRangeValue.Count == 0;
        }
        #endregion


        #region Validate Is Active
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
        #endregion


        #region Validate Is Purchase Item
        private bool ValidateIsPurchaseItem()
        {
            if (_operation == CrudOperations.Delete) return true;

            var (cell, _) = GetGetterAndConverter(Keys.DefaultPurchaseQty);
            var rows = GetRecords();
            var invalidRows = rows.Where(x => !string.IsNullOrWhiteSpace(cell(x)) && cell(x)!.ToLower() is not "true" or "false").ToList();
            foreach (var row in invalidRows)
            {
                row.AddError(GetError("FailedToConvertAsBoolean.Message", cell(row)));
            }

            return invalidRows.Count == 0;
        }
        #endregion


        #region Validate Is Sales Item
        private bool ValidateIsSalesItem()
        {
            if (_operation == CrudOperations.Delete) return true;

            var (cell, _) = GetGetterAndConverter(Keys.IsSaleItem);
            var rows = GetRecords();
            var invalidRows = rows.Where(x => !string.IsNullOrWhiteSpace(cell(x)) && cell(x)!.ToLower() is not "true" or "false").ToList();
            foreach (var row in invalidRows)
            {
                row.AddError(GetError("FailedToConvertAsBoolean.Message", cell(row)));
            }

            return invalidRows.Count == 0;
        }
        #endregion


        #region Validate Category
        private bool ValidateCategory()
        {
            if (_operation == CrudOperations.Delete) return true;

            var validateExist = CheckCategorytExist();
            var validateEmpty = CheckEmtyCategory();
            return validateExist && validateEmpty;

        }

        private bool CheckCategorytExist()
        {
            var (cell, _) = GetGetterAndConverter(Keys.CategoryName);
            var rows = GetRecords();
            var inputNames = GetInputValues(Keys.CategoryName).Select(x => (string)x!).ToList();
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
            var (cell, _) = GetGetterAndConverter(Keys.CategoryName);
            var isAnyError = false;
            var rows = GetRecords();
            var columnKey = _fieldMapping[Keys.CategoryName];
            var headerName = GetExcelHeader(columnKey);

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError(RequiredMessageResourceKey, headerName));
                isAnyError = true;
            }

            return !isAnyError;
        }
        #endregion


        #region Validate Unit
        private bool ValidateUnit()
        {
            if (_operation == CrudOperations.Delete) return true;

            var validateExist = CheckUnitExist();
            var validateEmpty = CheckEmtyUnit();
            return validateExist && validateEmpty;

        }

        private bool CheckUnitExist()
        {
            var (cell, _) = GetGetterAndConverter(Keys.UnitName);
            var rows = GetRecords();
            var inputNames = GetInputValues(Keys.UnitName).Select(x => (string)x!).ToList();
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
            var (cell, _) = GetGetterAndConverter(Keys.UnitName);
            var isAnyError = false;
            var rows = GetRecords();
            var columnKey = _fieldMapping[Keys.UnitName];
            var headerName = GetExcelHeader(columnKey);

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError(RequiredMessageResourceKey, headerName));
                isAnyError = true;
            }

            return !isAnyError;
        }
        #endregion


        #region Validate SKU Code
        private bool ValidateSKU()
        {
            if (_operation == CrudOperations.Delete) return true;

            var validEmptyCheck = CheckEmptySKU();
            var validDuplicateCheck = CheckDulicateSKU();
            return validEmptyCheck && validDuplicateCheck;
        }


        bool CheckEmptySKU()
        {
            var (cell, _) = GetGetterAndConverter(Keys.SKU);
            var isAnyError = false;
            var rows = GetRecords();
            var columnKey = _fieldMapping[Keys.SKU];
            var headerName = GetExcelHeader(columnKey);

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError(RequiredMessageResourceKey, headerName));
                isAnyError = true;
            }

            return !isAnyError;
        }

        bool CheckDulicateSKU()
        {
            var (idCell, idConverter) = GetGetterAndConverter(Keys.Id);
            var (nameCell, _) = GetGetterAndConverter(Keys.SKU);
            var rows = GetRecords();
            var rowMapping = rows.Select(x => new { Id = idConverter(idCell(x) ?? Guid.NewGuid().ToString()), Code = nameCell(x), Row = x }).Where(x => !string.IsNullOrWhiteSpace(x.Code)).ToList();
            var ids = rowMapping.Select(x => x.Id).ToList();
            var codes = rowMapping.Select(x => x.Code).ToList();
            var predicateDuplicateSKU = _dbContext.SKUItems.GetAll(_actor).Where(x => !ids.Contains(x.Id) && codes.Contains(x.SKU)).Select(x => x.SKU!.ToLower()).ToList();
            var invalidRows = rowMapping.Where(x => predicateDuplicateSKU.Contains(x.Code!.ToLower()));
            foreach (var row in invalidRows)
            {
                row.Row.AddError(GetError(AlreadyExistMessageResourceKey, row.Code));
            }
            return !invalidRows.Any();
        }

        #endregion

        
        #region Validate Name
        bool ValidateItemName()
        {
            if (_operation == CrudOperations.Delete) return true;

            var validEmptyCheck = CheckEmptyItemName();
            return validEmptyCheck;
        }

        bool CheckEmptyItemName()
        {
            var (cell, _) = GetGetterAndConverter(Keys.ItemName);
            var isAnyError = false;
            var rows = GetRecords();
            var columnKey = _fieldMapping[Keys.ItemName];
            var headerName = GetExcelHeader(columnKey);

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError(RequiredMessageResourceKey, headerName));
                isAnyError = true;
            }

            return !isAnyError;
        }
        #endregion


        #region Validate Variant name
        bool ValidateVariantName()
        {
            if (_operation == CrudOperations.Delete) return true;

            var validEmptyCheck = CheckEmptyVariantName();
            var validDuplicateCheck = CheckDuplicateVariantName();
            return validEmptyCheck && validDuplicateCheck;
        }

        bool CheckEmptyVariantName()
        {
            var (cell, _) = GetGetterAndConverter(Keys.VarianName);
            var isAnyError = false;
            var rows = GetRecords();
            var columnKey = _fieldMapping[Keys.VarianName];
            var headerName = GetExcelHeader(columnKey);

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(cell(x))))
            {
                row.AddError(GetError(RequiredMessageResourceKey, headerName));
                isAnyError = true;
            }

            return !isAnyError;
        }


        bool CheckDuplicateVariantName()
        {
            var (idCell, idConverter) = GetGetterAndConverter(_columnId);
            var (nameCell, _) = GetGetterAndConverter(Keys.VarianName);
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
        #endregion


        #region Convert string to GUID
        static Guid? StringToGuid(string? str)
        {
            if (str == null) return null;
            if (Guid.TryParse(str, out var id)) return id;
            return null;
        }
        #endregion
    
    }
}
