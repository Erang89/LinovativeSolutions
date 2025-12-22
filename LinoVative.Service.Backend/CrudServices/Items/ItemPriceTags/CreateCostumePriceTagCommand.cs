using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemPriceTags
{
    public class CreateCostumePriceTagCommand : CostumePriceTagDto, IRequest<Result>
    {
    }

    public class CreateCostumePriceTagHandlerService : SaveNewServiceBase<CostumePriceTag, CreateCostumePriceTagCommand>
    {
        
        public CreateCostumePriceTagHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }


        protected override async Task<Result> Validate(CreateCostumePriceTagCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            var isNameExist = GetAll().Where(x => x.Name!.Contains(request.Name!)).Any();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return result;
        }
    }
}
