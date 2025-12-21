using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Service.Core.Shifts;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Shifts;
using LinoVative.Shared.Dto.Outlets;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Shifts
{
    public class CreateShiftCommand : ShiftDto, IRequest<Result>
    {
        public List<OutletShiftDto> Outlets { get; set; } = new();
    }

    public class CreateShiftHandlerService : SaveNewServiceBase<Shift, CreateShiftCommand>
    {
        
        public CreateShiftHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }

        public override async Task BeforeSave(CreateShiftCommand request, Shift entity, CancellationToken token)
        {
            var maxSequence = await _dbContext.OutletShifts
                .GetAll(_actor)
                .Where(x => x.Outlet!.CompanyId == _actor.CompanyId)
                .GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) })
                .ToListAsync();

            foreach (var os in request.Outlets)
            {
                var newOs = _mapper.Map<OutletShift>(os);
                newOs.ShiftId = entity.Id;
                newOs.CreateBy(_actor);
                newOs.StartTime = entity.StartTime;
                newOs.EndTime = entity.EndTime;
                newOs.Sequence = (maxSequence.FirstOrDefault(x => x.Id == os.OutletId)?.Max ?? 0) + 1;
                _dbContext.OutletShifts.Add(newOs);
            }

        }

        protected override async Task<Result> Validate(CreateShiftCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            var anyDuplicateOutlet = request.Outlets.GroupBy(x => x.OutletId).Any(x => x.Count() > 1);
            if (anyDuplicateOutlet)
                return Result.Failed("Duplicate Outlet ID is not allowed");

            var outletIds = request.Outlets.Select(x => x.OutletId).Distinct();
            var outletIdsCount = await _dbContext.Outlets.CountAsync(x => outletIds.Contains(x.Id));
            if (outletIds.Count() != outletIdsCount)
                return Result.Failed("Some outlet ID are not in the system");

            var isNameExist = await GetAll().Where(x => x.CompanyId ==  _actor.CompanyId && x.Name!.Contains(request.Name!)).AnyAsync();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return result;
        }
    }
}
