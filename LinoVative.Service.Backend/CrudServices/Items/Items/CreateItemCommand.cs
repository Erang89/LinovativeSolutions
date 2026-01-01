using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Items.Items.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.Items
{
    public class CreateItemCommand : ItemInputDto, IRequest<Result>
    {

    }

    public class CreateItemHandlerService : SaveNewServiceBase<Item, CreateItemCommand>
    {

        private readonly IItemValidator _validator;
        public CreateItemHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, IItemValidator validator) : base(dbContext, actor, mapper, appCache, localizer)
        {
            _validator = validator;
        }


        public override async Task BeforeSave(CreateItemCommand request, Item entity, CancellationToken token)
        {
            //var category = _mapper.Map<ItemCategory>(request.Category!);
            //_dbContext.ItemCategories.Add(category);
            //entity.CategoryId = category.Id;

            //var unit = _mapper.Map<ItemUnit>(request.Unit!);
            //_dbContext.ItemUnits.Add(unit);
            //entity.UnitId = unit.Id;

            //foreach (var dto in request.ItemPriceTypes)
            //{
            //    var newPrice = _mapper.Map<ItemPriceType>(dto);
            //    newPrice.CreateBy(_actor);
            //    _dbContext.ItemPriceTypes.Add(newPrice);
            //}

            await Task.CompletedTask;
        }


        protected override async Task<Result> Validate(CreateItemCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);

            var isNameExist = GetAll().Where(x => x.Name!.Contains(request.Name!)).Any();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return await _validator.Validate(request, token);
        }
    }
}
