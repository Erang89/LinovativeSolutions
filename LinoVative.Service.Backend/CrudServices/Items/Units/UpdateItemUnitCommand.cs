using Linovative.Shared.Interface;
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
        public UpdateItemUnitHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

        protected override string LocalizerPrefix => nameof(ItemUnitDto);

        public Task<Result> Handle(UpdateItemUnitCommand request, CancellationToken ct) => base.SaveUpdate(request, ct);

        protected override async Task<Result> ValidateSaveUpdate(UpdateItemUnitCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);

            var isNameExist = GetAll().Where(x => x.Name!.Contains(request.Name!) && x.Id != request.Id).Any();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return result;
        }
    }
}
