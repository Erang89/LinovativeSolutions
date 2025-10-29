using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemCategories
{
    public class UpdateItemCategoryCommand : ItemCategoryDto, IRequest<Result>
    {
    }

    public class UpdateItemCategoryHandlerService : SaveUpdateServiceBase<ItemCategory, UpdateItemCategoryCommand>, IRequestHandler<UpdateItemCategoryCommand, Result>
    {
        public UpdateItemCategoryHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        public Task<Result> Handle(UpdateItemCategoryCommand request, CancellationToken ct) => base.SaveUpdate(request, ct);
    }
}
