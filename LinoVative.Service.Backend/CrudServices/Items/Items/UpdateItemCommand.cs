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
        private readonly IItemHelperService _itemHelper;


        public UpdateItemHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, IItemValidator validator, IItemHelperService helper) :
            base(dbContext, actor, mapper, appCache, localizer)
        {
            _validator = validator;
            _itemHelper = helper;
        }


        protected override async Task BeforeSaveUpdate(UpdateItemCommand request, Item entity, CancellationToken token)
        {
            await base.BeforeSaveUpdate(request, entity, token);
            entity.UpdateItemNameInOtherTable(_dbContext);
            await _itemHelper.SaveItemData(request, entity, token);
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
