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

namespace LinoVative.Service.Backend.CrudServices.Items.ItemGroups
{
    public class UpdateItemGroupCommand : ItemGroupInputDto, IRequest<Result>
    {
    }

    public class UpdateItemGroupHandlerService : SaveUpdateServiceBase<ItemGroup, UpdateItemGroupCommand>, IRequestHandler<UpdateItemGroupCommand, Result>
    {
        public UpdateItemGroupHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        protected override async Task BeforeSaveUpdate(UpdateItemGroupCommand request, ItemGroup entity, CancellationToken token)
        {
            var opm = await _dbContext.OutletItemGroups.GetAll(_actor).Where(x => x.ItemGroupId == entity.Id).ToListAsync();
            var maxSequence = await _dbContext.OutletItemGroups
                .GetAll(_actor)
                .Where(x => x.Outlet!.CompanyId == _actor.CompanyId)
                .GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) }).ToListAsync();

            foreach (var dto in request.OutletGroups)
            {
                var existing = opm.FirstOrDefault(x => x.Id == dto.Id);
                if (existing is not null)
                {
                    _mapper.Map(dto, existing);
                    if (_dbContext.GetEntityState(existing) == EntityState.Modified)
                        existing.ModifyBy(_actor);
                    continue;
                }

                var newOt = _mapper.Map<OutletItemGroup>(dto);
                newOt.ItemGroupId = entity.Id;
                newOt.Sequence = (maxSequence.FirstOrDefault(x => x.Id == dto.OutletId)?.Max ?? 0) + 1;
                _dbContext.OutletItemGroups.Add(newOt);
            }
        }

        protected override async Task<Result> ValidateSaveUpdate(UpdateItemGroupCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);

            var isNameExist = await GetAll().Where(x => x.Name!.Contains(request.Name!) && x.Id != request.Id).AnyAsync();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            var anyDuplicateOutlet = request.OutletGroups.GroupBy(x => x.OutletId).Any(x => x.Count() > 1);
            if (anyDuplicateOutlet)
                return Result.Failed("Duplicate Outlet ID is not allowed");

            var outletIds = request.OutletGroups.Select(x => x.OutletId).Distinct();
            var outletIdsCount = await _dbContext.Outlets.CountAsync(x => outletIds.Contains(x.Id));
            if (outletIds.Count() != outletIdsCount)
                return Result.Failed("Some outlet ID are not in the system");

            return result;
        }
    }
}
