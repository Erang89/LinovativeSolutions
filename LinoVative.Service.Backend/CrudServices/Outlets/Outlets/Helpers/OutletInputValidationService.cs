using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Outlets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Outlets.Helpers
{
    public interface IOutletInputValidationService
    {
        public Task<Result> Validate(OutletDto request, CancellationToken token);
    }

    public class OutletInputValidationService : IOutletInputValidationService
    {
        private readonly IAppDbContext _dbContext;
        private readonly IActor _actor;
        private readonly ILangueageService _lang;
        private readonly IStringLocalizer _localizer;

        public OutletInputValidationService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IStringLocalizer localizer)
        {
            _dbContext = dbContext;
            _actor = actor;
            _lang = lang;
            _localizer = localizer;
            _lang.EnsureLoad(AvailableLanguageKeys.CreateOutletCommand);
        }


        private string GetEntityName(string entity) => _lang[$"Entity.Name.{entity}"];
        private string GetErrorMessage(string key, params object[] args) => 
            string.Format(_lang[$"{AvailableLanguageKeys.CreateOrUpdateOutletAreaCommand}.{key}"], args);

        protected static void AddError(Result result, string propertyKey, string errorMessage) =>
            result.AddInvalidProperty(propertyKey, errorMessage);


        public async Task<Result> Validate(OutletDto request, CancellationToken token)
        {

            const string dupliCateMessageKey = "DuplicateEntity.Message";

            // Critical Check
            var duplicateShifts = request.Shifts?.GroupBy(x => x.ShiftId).Select(x => new { Id = x.Key, Count = x.Count() }).Where(x => x.Count > 1).ToList();
            if (duplicateShifts?.Count > 0) return Result.Failed(GetErrorMessage(dupliCateMessageKey, GetEntityName("Shift")));

            var duplicateBankNote = request.BankNotes.GroupBy(x => x.BankNoteId).Select(x => new { Id = x.Key, Count = x.Count() }).Where(x => x.Count > 1).ToList();
            if (duplicateBankNote.Count > 0) return Result.Failed(GetErrorMessage(dupliCateMessageKey, GetEntityName("BankNote")));

            var duplicatePaymentMethod = request.PaymentMethods.GroupBy(x => x.PaymentMethodId).Select(x => new { Id = x.Key, Count = x.Count() }).Where(x => x.Count > 1).ToList();
            if (duplicatePaymentMethod.Count > 0) return Result.Failed(GetErrorMessage(dupliCateMessageKey, GetEntityName("PaymentMethod")));

            var duplicateOrderTyepe = request.OrderTypes.GroupBy(x => x.OrderTypeId).Select(x => new { Id = x.Key, Count = x.Count() }).Where(x => x.Count > 1).ToList();
            if (duplicateOrderTyepe.Count > 0) return Result.Failed(GetErrorMessage(dupliCateMessageKey, GetEntityName("OrderType")));

            var duplicateItemGroups = request.ItemGroups.GroupBy(x => x.ItemGroupId).Select(x => new { Id = x.Key, Count = x.Count() }).Where(x => x.Count > 1).ToList();
            if (duplicateItemGroups.Count > 0) return Result.Failed(GetErrorMessage(dupliCateMessageKey, GetEntityName("ItemGroup")));

            var duplicateItemCategory = request.ItemCategories.GroupBy(x => x.ItemCategoryId).Select(x => new { Id = x.Key, Count = x.Count() }).Where(x => x.Count > 1).ToList();
            if (duplicateItemCategory.Count > 0) return Result.Failed(GetErrorMessage(dupliCateMessageKey, GetEntityName("ItemCategory")));

            var result = await CheckDataSource(request);
            if (!result) return result;


            await ValidateInputOrderTypes(result, request);
            return result;
        }

        async Task<Result> CheckDataSource(OutletDto request)
        {
            // Check Shift
            var shiftIds = request.Shifts.Select(x => x.ShiftId).Distinct().ToList();
            var existingCount = await _dbContext.Shifts.GetAll(_actor).CountAsync(x => shiftIds.Contains(x.Id));
            var anyNotExist = existingCount != shiftIds.Count;
            if (anyNotExist) return Result.Failed(GetErrorMessage("SomeShiftIdNotExist"));

            // Check Bank Notes
            var bankNoteIds = request.BankNotes.Select(x => x.BankNoteId).Distinct().ToList();
            existingCount = await _dbContext.BankNotes.GetAll(_actor).CountAsync(x => bankNoteIds.Contains(x.Id));
            anyNotExist = existingCount != bankNoteIds.Count;
            if (anyNotExist) return Result.Failed(GetErrorMessage("SomeBankNotetIdNotExist"));

            // Check Payment Methods
            var paymentMethodIds = request.PaymentMethods.Select(x => x.PaymentMethodId).Distinct().ToList();
            existingCount = await _dbContext.PaymentMethods.GetAll(_actor).CountAsync(x => paymentMethodIds.Contains(x.Id));
            anyNotExist = existingCount != paymentMethodIds.Count;
            if (anyNotExist) return Result.Failed(GetErrorMessage("SomePaymentMethodIdNotExist"));

            // Check Order Types
            var orderTypeIds = request.OrderTypes.Select(x => x.OrderTypeId).Distinct().ToList();
            existingCount = await _dbContext.OrderTypes.GetAll(_actor).CountAsync(x => orderTypeIds.Contains(x.Id));
            anyNotExist = existingCount != orderTypeIds.Count;
            if (anyNotExist) return Result.Failed(GetErrorMessage("SomeOrderTypeIdNotExist"));

            // Check Item Group
            var itemGroupIds = request.ItemGroups.Select(x => x.ItemGroupId).Distinct().ToList();
            existingCount = await _dbContext.ItemGroups.GetAll(_actor).CountAsync(x => itemGroupIds.Contains(x.Id));
            anyNotExist = existingCount != itemGroupIds.Count;
            if (anyNotExist) return Result.Failed(GetErrorMessage("SomeItemGroupIdNotExist"));

            // Check Item Category
            var itemCategoryIds = request.ItemCategories.Select(x => x.ItemCategoryId).Distinct().ToList();
            existingCount = await _dbContext.ItemCategories.GetAll(_actor).CountAsync(x => itemCategoryIds.Contains(x.Id));
            anyNotExist = existingCount != itemCategoryIds.Count;
            if (anyNotExist) return Result.Failed(GetErrorMessage("SomeItemCategoryIdNotExist"));

            // Check Item for exception
            var itemIds = request.OutletItemExceptionals.Where(x => x.Type == ItemExceptionTypes.Item).Select(x => x.EntityId).Distinct().ToList();
            existingCount = await _dbContext.Items.GetAll(_actor).CountAsync(x => itemIds.Contains(x.Id));
            anyNotExist = existingCount != itemIds.Count;
            if (anyNotExist) return Result.Failed(GetErrorMessage("SomeItemIdNotExist"));

            // Check Item Group for Exception
            itemGroupIds = [.. request.OutletItemExceptionals.Where(x => x.Type == ItemExceptionTypes.Group).Select(x => (Guid?)x!.EntityId).Distinct()];
            existingCount = await _dbContext.ItemGroups.GetAll(_actor).CountAsync(x => itemGroupIds.Contains(x.Id));
            anyNotExist = existingCount != itemGroupIds.Count;
            if (anyNotExist) return Result.Failed(GetErrorMessage("SomeItemGroupIdNotExist"));

            // Check Item Category for Exception
            itemCategoryIds = [.. request.OutletItemExceptionals.Where(x => x.Type == ItemExceptionTypes.Category).Select(x => (Guid?)x!.EntityId).Distinct()];
            existingCount = await _dbContext.ItemCategories.GetAll(_actor).CountAsync(x => itemCategoryIds.Contains(x.Id));
            anyNotExist = existingCount != itemCategoryIds.Count;
            if (anyNotExist) return Result.Failed(GetErrorMessage("SomeItemCategoryIdNotExist"));

            return Result.OK();
        }

        async Task ValidateInputOrderTypes(Result result, OutletDto request)
        {
            var sequence = 0;
            foreach (var orderType in request!.OrderTypes)
            {
                if (orderType.TaxPercent < 0 || orderType.TaxPercent > 100)
                    AddError(result, $"{nameof(request.OrderTypes)}[{sequence}].{nameof(OutletOrderTypeDto.TaxPercent)}",
                        GetErrorMessage("TaxPercentOutOfRange.Message"));

                if (orderType.ServicePercent < 0 || orderType.ServicePercent > 100)
                    AddError(result, $"{nameof(request.OrderTypes)}[{sequence}].{nameof(OutletOrderTypeDto.TaxPercent)}",
                        GetErrorMessage("ServicePercentOutOfRange.Message"));
            }

            await Task.CompletedTask;
        }
    }
}
