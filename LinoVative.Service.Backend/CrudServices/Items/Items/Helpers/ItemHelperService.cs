using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.Items.Helpers
{
    public interface IItemHelperService
    {
        Task SaveItemData(ItemInputDto request, Item entity, CancellationToken token);
    }


    public class ItemHelperService : IItemHelperService
    {
        private readonly IAppDbContext _dbContext;
        private readonly IActor _actor;
        private readonly IMapper _mapper;

        public ItemHelperService(IAppDbContext dbContext, IActor actor, ILangueageService lang, IMapper mapper)
        {
            _dbContext = dbContext;
            _actor = actor;
            lang.EnsureLoad(AvailableLanguageKeys.InputItems);
            _mapper = mapper;
        }

        public async Task SaveItemData(ItemInputDto request, Item entity, CancellationToken token)
        {

            var skuIds = request.SKUItems.Select(x => x.Id).ToList();
            var skuItems = await _dbContext.SKUItems.GetAll(_actor).Where(x => skuIds.Contains(x.Id)).ToListAsync();
            var customePrices = await _dbContext.ItemPriceTypes.GetAll(_actor).Where(x => skuIds.Contains(x.SKUItemId!.Value)).ToListAsync();

            foreach (var dto in request.SKUItems)
            {
                var skuItem = skuItems.FirstOrDefault(x => x.Id == dto.Id);
                if (skuItem is not null)
                {
                    _mapper.Map(dto, skuItem);
                    if (_dbContext.GetEntityState(skuItem) == EntityState.Modified)
                        skuItem.ModifyBy(_actor);

                    await MappingCustomePrice(skuItem, customePrices, dto);
                    skuItem.HasCostumePrice = dto.CostumePrices.Any(x => x.IsActive);
                    continue;
                }

                skuItem = _mapper.Map<SKUItem>(dto);
                skuItem.ItemId = entity.Id;
                skuItem.CreateBy(_actor);
                skuItem.HasCostumePrice = dto.CostumePrices.Any(x => x.IsActive);
                _dbContext.SKUItems.Add(skuItem);
                await MappingCustomePrice(skuItem, customePrices, dto);
            }
        }

        async Task MappingCustomePrice(SKUItem skuItem, List<ItemPriceType> priceTypes, SKUItemInputDto skuItemDto)
        {
            foreach (var dto in skuItemDto.CostumePrices)
            {
                var priceType = priceTypes.FirstOrDefault(x => x.Id == dto.Id);
                if (priceType is not null)
                {
                    priceType.SKUItemId = skuItem.Id;
                    _mapper.Map(dto, priceType);
                    continue;
                }

                priceType = _mapper.Map<ItemPriceType>(dto);
                priceType.CreateBy(_actor);
                priceType.SKUItemId = skuItem.Id;
                _dbContext.ItemPriceTypes.Add(priceType);
            }
        }
    }
}
