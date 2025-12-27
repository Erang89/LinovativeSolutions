using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Items.Items.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.Items
{
    public class UpdateItemCommand : ItemInputDto, IRequest<Result>
    {
    }

    public class UpdateItemHandlerService : SaveUpdateServiceBase<Item, UpdateItemCommand>
    {
        private readonly IItemValidator _validator;
        public UpdateItemHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, IItemValidator validator) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
            _validator = validator;
        }


        protected override async Task BeforeSaveUpdate(UpdateItemCommand request, Item entity, CancellationToken token)
        {
            await base.BeforeSaveUpdate(request, entity, token);
            entity.UpdateItemNameInOtherTable(_dbContext);

            var priceTypes = await _dbContext.ItemPriceTypes.Where(x => !x.IsDeleted && x.ItemId == entity.Id).ToListAsync(token);

            foreach(var dto in request.ItemPriceTypes)
            {
                var exisiting = priceTypes.FirstOrDefault(x => x.PriceTypeId == dto.PriceTypeId);
                if(exisiting is not null)
                {
                    _mapper.Map(dto, exisiting);
                    continue;
                }

                var newPrice = _mapper.Map<ItemPriceType>(dto);
                newPrice.CreateBy(_actor);
                _dbContext.ItemPriceTypes.Add(newPrice);
            }
        }

        protected override async Task<Result> ValidateSaveUpdate(UpdateItemCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);

            var isNameExist = await GetAll().Where(x => x.Name!.Contains(request.Name!) && x.Id != request.Id).AnyAsync();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return await _validator.Validate(request, token);
        }
    }
}
