using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.Units
{
    public class CreateItemUnitCommand : ItemUnitDto, IRequest<Result>
    {
    }

    public class CreateItemUnitHandlerService : SaveNewServiceBase<ItemUnit, CreateItemUnitCommand>, IRequestHandler<CreateItemUnitCommand, Result>
    {
        public CreateItemUnitHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
        }

        protected override string LocalizerPrefix => nameof(ItemUnitDto);

        public Task<Result> Handle(CreateItemUnitCommand request, CancellationToken ct) => base.SaveNew(request, ct);
    }
}
