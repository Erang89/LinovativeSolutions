using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemCategories
{
    public class CreateItemCategoryCommand : ItemCategoryInputDto, IRequest<Result>
    {

    }

    public class CreateItemCategoryHandlerService : SaveNewServiceBase<ItemCategory, CreateItemCategoryCommand>
    {
        public CreateItemCategoryHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
      
        }

        public override async Task BeforeSave(CreateItemCategoryCommand request, ItemCategory entity, CancellationToken token)
        {
            var maxSequence = await _dbContext.OutletItemCategories
                .GetAll(_actor)
                .Where(x => x.Outlet!.CompanyId == _actor.CompanyId)
                .GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) }).ToListAsync();

            foreach (var os in request.OutletCategories)
            {
                var group = _mapper.Map<OutletItemCategory>(os);
                group.ItemCategoryId = entity.Id;
                group.CreateBy(_actor);
                group.Sequence = (maxSequence.FirstOrDefault(x => x.Id == os.OutletId)?.Max ?? 0) + 1;
                _dbContext.OutletItemCategories.Add(group);
            }
        }

        protected override async Task<Result> Validate(CreateItemCategoryCommand request, CancellationToken token)
        {
            var validate  = await base.Validate(request, token);
            if (!validate) return validate;

            var isExist = await GetAll().Where(x => x.GroupId == request.GroupId && x.Name == request.Name).AnyAsync();
            if (isExist) Result.Failed(_localizer[$"ItemCategoryDto.Unique.Name.ErrorMessage", request.Name!]);


            var anyDuplicateOutlet = request.OutletCategories.GroupBy(x => x.OutletId).Any(x => x.Count() > 1);
            if (anyDuplicateOutlet)
                return Result.Failed("Duplicate Outlet ID is not allowed");

            var outletIds = request.OutletCategories.Select(x => x.OutletId).Distinct();
            var outletIdsCount = await _dbContext.Outlets.CountAsync(x => outletIds.Contains(x.Id));
            if (outletIds.Count() != outletIdsCount)
                return Result.Failed("Some outlet ID are not in the system");



            return Result.OK();
        }

    }
}
