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
    public class CreatePaymenMethoCommand : PaymentMethodUpdateDto, IRequest<Result>
    {

    }

    public class CreatePaymentMethodHandlerService : SaveNewServiceBase<PaymentMethod, CreatePaymenMethoCommand>
    {
        
        public CreatePaymentMethodHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }


        public override async Task BeforeSave(CreatePaymenMethoCommand request, PaymentMethod entity, CancellationToken token)
        {
            var maxSequence = await _dbContext.OutletPaymentMethods
                .GetAll(_actor)
                .Where(x => x.Outlet!.CompanyId == _actor.CompanyId)
                .GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) })
                .ToListAsync();

            foreach (var dto in request.OutletPaymentMethods)
            {
                var opm = _mapper.Map<OutletPaymentMethod>(dto);
                opm.PaymentMethodId = entity.Id;
                opm.Sequence = (maxSequence.FirstOrDefault(x => x.Id == dto.OutletId)?.Max ?? 0) + 1;
                opm.CreateBy(_actor);
                _dbContext.OutletPaymentMethods.Add(opm);
            }
        }


        protected override async Task<Result> Validate(CreatePaymenMethoCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

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
