using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Backend.LocalizerServices;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.Units
{
    public class UpdateItemUnitCommand : ItemUnitDto, IRequest<Result>
    {
    }

    public class UpdateItemUnitHandlerService : SaveUpdateServiceBase<ItemUnit, UpdateItemUnitCommand>, IRequestHandler<UpdateItemUnitCommand, Result>
    {
        public UpdateItemUnitHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, ILanguageService langService) : 
            base(dbContext, actor, mapper, appCache, localizer, langService)
        {
        }

        protected override string LocalizerPrefix => nameof(ItemUnitDto);

        public Task<Result> Handle(UpdateItemUnitCommand request, CancellationToken ct) => base.SaveUpdate(request, ct);
    }
}
