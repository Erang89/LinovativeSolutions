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
    public class UpdateItemCategoryCommand : ItemCategoryInputDto, IRequest<Result>
    {
    }

    public class UpdateItemCategoryHandlerService : SaveUpdateServiceBase<ItemCategory, UpdateItemCategoryCommand>
    {
        public UpdateItemCategoryHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        protected override async Task BeforeSaveUpdate(UpdateItemCategoryCommand request, ItemCategory entity, CancellationToken token)
        {
            var opm = await _dbContext.OutletItemCategories.GetAll(_actor).Where(x => x.ItemCategoryId == entity.Id).ToListAsync();
            var maxSequence = await _dbContext.OutletItemCategories
                .GetAll(_actor)
                .Where(x => x.Outlet!.CompanyId == _actor.CompanyId)
                .GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) }).ToListAsync();

            foreach (var dto in request.OutletCategories)
            {
                var existing = opm.FirstOrDefault(x => x.Id == dto.Id);
                if (existing is not null)
                {
                    _mapper.Map(dto, existing);
                    if (_dbContext.GetEntityState(existing) == EntityState.Modified)
                        existing.ModifyBy(_actor);
                    continue;
                }

                var newEntity = _mapper.Map<OutletItemCategory>(dto);
                newEntity.ItemCategoryId = entity.Id;
                newEntity.Sequence = (maxSequence.FirstOrDefault(x => x.Id == dto.OutletId)?.Max ?? 0) + 1;
                _dbContext.OutletItemCategories.Add(newEntity);
            }
        }

        protected override async Task<Result> ValidateSaveUpdate(UpdateItemCategoryCommand request, CancellationToken token)
        {
            var validate = await base.ValidateSaveUpdate(request, token);
            if (!validate) return validate;


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
