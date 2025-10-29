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
    public class CreateItemCategoryCommand : ItemCategoryDto, IRequest<Result>
    {

    }

    public class CreateItemCategoryHandlerService : SaveNewServiceBase<ItemCategory, CreateItemCategoryCommand>, IRequestHandler<CreateItemCategoryCommand, Result>
    {
        public CreateItemCategoryHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
      
        }

        public Task<Result> Handle(CreateItemCategoryCommand request, CancellationToken ct) => base.SaveNew(request, ct);

        protected override async Task<Result> Validate(CreateItemCategoryCommand request, CancellationToken token)
        {
            var validate  = await base.Validate(request, token);
            if (!validate) return validate;

            var isExist = GetAll().Where(x => x.GroupId == request.GroupId && x.Name == request.Name).Any();
            if (isExist) Result.Failed(_localizer[$"ItemCategoryDto.Unique.Name.ErrorMessage", request.Name!]);


            return Result.OK();
        }

    }
}
