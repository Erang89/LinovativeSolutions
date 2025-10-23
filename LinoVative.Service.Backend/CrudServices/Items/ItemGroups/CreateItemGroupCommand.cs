using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Backend.LocalizerServices;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemGroups
{
    public class CreateItemGroupCommand : ItemGroupDto, IRequest<Result>
    {

    }

    public class CreateItemGroupHandlerService : SaveNewServiceBase<ItemGroup, CreateItemGroupCommand>, IRequestHandler<CreateItemGroupCommand, Result>
    {
        public CreateItemGroupHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
        }

        protected override string LocalizerPrefix => nameof(ItemGroupDto);

        public Task<Result> Handle(CreateItemGroupCommand request, CancellationToken ct) => base.SaveNew(request, ct);

        protected override async Task<Result> Validate(CreateItemGroupCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);

            var isNameExist = GetAll().Where(x => x.Name!.Contains(request.Name!)).Any();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return result;
        }
    }
}
