using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.Items
{
    public class UpdateItemCommand : ItemDto, IRequest<Result>
    {
    }

    public class UpdateItemHandlerService : SaveUpdateServiceBase<Item, UpdateItemCommand>, IRequestHandler<UpdateItemCommand, Result>
    {
        public UpdateItemHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        protected override async Task<Result> ValidateSaveUpdate(UpdateItemCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);

            var isNameExist = GetAll().Where(x => x.Name!.Contains(request.Name!) && x.Id != request.Id).Any();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return result;
        }
    }
}
