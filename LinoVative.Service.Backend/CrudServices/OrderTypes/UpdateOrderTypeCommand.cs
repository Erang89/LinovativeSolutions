using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.OrderTypes;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.OrderTypes;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.OrderTypes
{
    public class UpdateOrderTypeCommand : OrderTypeInputDto, IRequest<Result>
    {
    }

    public class UpdateOrderTypeHandlerService : SaveUpdateServiceBase<OrderType, UpdateOrderTypeCommand>
    {
        public UpdateOrderTypeHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {

        }


        protected override async Task BeforeSaveUpdate(UpdateOrderTypeCommand request, OrderType entity, CancellationToken token)
        {
            var opm = await _dbContext.OutletOrderTypes.GetAll(_actor).Where(x => x.OrderTypeId == entity.Id).ToListAsync();
            var maxSequence = await _dbContext.OutletPaymentMethods
                .GetAll(_actor)
                .Where(x => x.Outlet!.CompanyId == _actor.CompanyId)
                .GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) })
                .ToListAsync();

            foreach (var dto in request.OutletOrderTypes)
            {
                var existing = opm.FirstOrDefault(x => x.Id == dto.Id);
                if (existing is not null)
                {
                    _mapper.Map(dto, existing);
                    if (_dbContext.GetEntityState(existing) == EntityState.Modified)
                        existing.ModifyBy(_actor);
                    continue;
                }

                var newOt = _mapper.Map<OutletOrderType>(dto);
                newOt.OrderTypeId = entity.Id;
                newOt.Sequence = (maxSequence.FirstOrDefault(x => x.Id == dto.OutletId)?.Max ?? 0) + 1;
                _dbContext.OutletOrderTypes.Add(newOt);
            }
        }

        protected override async Task<Result> ValidateSaveUpdate(UpdateOrderTypeCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);

            var anyDuplicateOutlet = request.OutletOrderTypes.GroupBy(x => x.OutletId).Any(x => x.Count() > 1);
            if (anyDuplicateOutlet)
                return Result.Failed("Duplicate Outlet ID is not allowed");

            var outletIds = request.OutletOrderTypes.Select(x => x.OutletId).Distinct();
            var outletIdsCount = await _dbContext.Outlets.CountAsync(x => outletIds.Contains(x.Id));
            if (outletIds.Count() != outletIdsCount)
                return Result.Failed("Some outlet ID are not in the system");

            return result;
        }
    }
}
