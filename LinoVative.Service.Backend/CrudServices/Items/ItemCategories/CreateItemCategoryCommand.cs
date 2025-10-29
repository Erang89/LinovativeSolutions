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
    public class CreateItemCategoryCommand : ItemGroupDto, IRequest<Result>
    {

    }

    public class CreateItemCategoryHandlerService : SaveNewServiceBase<ItemCategory, CreateItemCategoryCommand>, IRequestHandler<CreateItemCategoryCommand, Result>
    {
        public CreateItemCategoryHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
      
        }

        protected override string LocalizerPrefix => nameof(ItemGroupDto);

        public Task<Result> Handle(CreateItemCategoryCommand request, CancellationToken ct) => base.SaveNew(request, ct);

    }
}
