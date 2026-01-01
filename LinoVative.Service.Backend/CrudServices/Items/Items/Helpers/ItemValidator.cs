using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.Items.Helpers
{
    public interface IItemValidator
    {
        public Task<Result> Validate(ItemInputDto request, CancellationToken token);
    }

    public class ItemValidatorService : IItemValidator
    {
        private readonly IAppDbContext _dbContext;
        private readonly IActor _actor;
        private readonly ILangueageService _lang;
        public ItemValidatorService(IAppDbContext dbContext, IActor actor, ILangueageService lang)
        {
            _dbContext = dbContext;
            _actor = actor;
            _lang = lang;
            lang.EnsureLoad(AvailableLanguageKeys.InputItems);
        }


        public async Task<Result> Validate(ItemInputDto request, CancellationToken token)
        {
            // Validate Custome Price
            //var anyDuplicate = request.ItemPriceTypes.GroupBy(x => x.PriceTypeId).Any(x => x.Count() > 1);
            //if (anyDuplicate)
            //    Result.Failed("Type ID must not duplicated");

            //var priceTypeIds = request.ItemPriceTypes.Select(x => x.PriceTypeId).Distinct().ToList();
            //var priceTypeCount = await _dbContext.PriceTypes.GetAll(_actor).Where(x => priceTypeIds.Contains(x.Id)).CountAsync();
            //if (priceTypeIds.Count != priceTypeCount)
            //    return Result.Failed("Some Price Type IDS not in the system");

            var result = Result.OK();
            //var i = 0;
            //foreach(var pt in request.ItemPriceTypes)
            //{
            //    if (pt.Price is null && request.HasSellingTaxAndService && pt.IsActive)
            //        result.AddInvalidProperty($"{nameof(request.ItemPriceTypes)}[{i}].{nameof(ItemPriceTypeDto.Price)}", _lang["InputItems.Price.Required.Message"]);
            //}

            return result;
        }
    }
}
