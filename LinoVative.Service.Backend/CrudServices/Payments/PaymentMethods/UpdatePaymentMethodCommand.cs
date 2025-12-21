using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Payments;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Payments.PaymentMethods
{
    public class UpdatePaymentMethodCommand : PaymentMethodUpdateDto, IRequest<Result>
    {
    }

    public class UpdatePaymentMethodHandlerService : SaveUpdateServiceBase<PaymentMethod, UpdatePaymentMethodCommand>
    {
        public UpdatePaymentMethodHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        protected override async Task BeforeSaveUpdate(UpdatePaymentMethodCommand request, PaymentMethod entity, CancellationToken token)
        {
            var opm = await _dbContext.OutletPaymentMethods.GetAll(_actor).Where(x => x.PaymentMethodId == entity.Id).ToListAsync();
            var maxSequence = await _dbContext.OutletPaymentMethods
                .GetAll(_actor)
                .Where(x => x.Outlet!.CompanyId == _actor.CompanyId)
                .GroupBy(x => x.OutletId).Select(x => new {Id = x.Key, Max = x.Max(s => s.Sequence)})
                .ToListAsync();

            foreach (var dto in request.OutletPaymentMethods)
            {
                var existing = opm.FirstOrDefault(x => x.Id == dto.Id);
                if (existing is not null)
                {
                    _mapper.Map(dto, existing);
                    if (_dbContext.GetEntityState(existing) == EntityState.Modified)
                        existing.ModifyBy(_actor);
                    continue;
                }

                var newOpm = _mapper.Map<OutletPaymentMethod>(dto);
                newOpm.PaymentMethodId = entity.Id;
                newOpm.Sequence = (maxSequence.FirstOrDefault(x => x.Id == dto.OutletId)?.Max ?? 0) + 1;
                _dbContext.OutletPaymentMethods.Add(newOpm);
            }
        }

        protected override async Task<Result> ValidateSaveUpdate(UpdatePaymentMethodCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);

            var anyDuplicateOutlet = request.OutletPaymentMethods.GroupBy(x => x.OutletId).Any(x => x.Count() > 1);
            if (anyDuplicateOutlet)
                return Result.Failed("Duplicate Outlet ID is not allowed");

            var outletIds = request.OutletPaymentMethods.Select(x => x.OutletId).Distinct();
            var outletIdsCount = await _dbContext.Outlets.CountAsync(x => outletIds.Contains(x.Id));
            if (outletIds.Count() != outletIdsCount)
                return Result.Failed("Some outlet ID are not in the system");


            return result;
        }
    }
}
