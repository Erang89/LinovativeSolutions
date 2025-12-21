using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Service.Core.Shifts;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Shifts;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Shifts
{
    public class UpdateShiftCommand : ShiftUpdateDto, IRequest<Result>
    {
    }

    public class UpdateShiftHandlerService : SaveUpdateServiceBase<Shift, UpdateShiftCommand>, IRequestHandler<UpdateShiftCommand, Result>
    {
        public UpdateShiftHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        protected override async Task BeforeSaveUpdate(UpdateShiftCommand request, Shift entity, CancellationToken token)
        {
            var outletShifts = await _dbContext.OutletShifts.GetAll(_actor).Where(x => x.ShiftId == entity.Id).ToListAsync();
            var maxSequence = await _dbContext.OutletShifts.GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) }).ToListAsync();
            foreach (var os in request.Outlets)
            {
                var existing = outletShifts.FirstOrDefault(x => x.Id == os.Id);
                if(existing is not null)
                {
                    _mapper.Map(os, existing);
                    if (_dbContext.GetEntityState(existing) == EntityState.Modified)
                        existing.ModifyBy(_actor);
                    continue;
                }

                var newOutletShift = _mapper.Map<OutletShift>(os);
                newOutletShift.ShiftId = entity.Id;
                newOutletShift.StartTime = entity.StartTime;
                newOutletShift.EndTime = entity.EndTime;
                newOutletShift.Sequence = (maxSequence.FirstOrDefault(x => x.Id == os.OutletId)?.Max ?? 0) + 1;
                _dbContext.OutletShifts.Add(newOutletShift);
            }
        }

        protected override async Task<Result> ValidateSaveUpdate(UpdateShiftCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);

            // Validate Format
            var anyDuplicateOutlet = request.Outlets.GroupBy(x => x.OutletId).Any(x => x.Count() > 1);
            if (anyDuplicateOutlet)
                return Result.Failed("Duplicate Outlet ID is not allowed");

            var outletIds = request.Outlets.Select(x => x.OutletId).Distinct();
            var outletIdsCount = await _dbContext.Outlets.CountAsync(x => outletIds.Contains(x.Id));
            if(outletIds.Count() != outletIdsCount)
                return Result.Failed("Some outlet ID are not in the system");

            var isNameExist = await GetAll().Where(x => x.Name!.Contains(request.Name!) && x.Id != request.Id).AnyAsync();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return result;
        }
    }
}
