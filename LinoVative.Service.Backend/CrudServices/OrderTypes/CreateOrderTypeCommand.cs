using Linovative.Shared.Interface;
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
    public class CreateOrderTypeCommand : OrderTypeInputDto, IRequest<Result>
    {
    }

    public class CreateOrderTypeHandlerService : SaveNewServiceBase<OrderType, CreateOrderTypeCommand>, IRequestHandler<CreateOrderTypeCommand, Result>
    {
        
        public CreateOrderTypeHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }

        public override async Task BeforeSave(CreateOrderTypeCommand request, OrderType entity, CancellationToken token)
        {
            var maxSequence = await _dbContext.OutletOrderTypes.GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) }).ToListAsync();

            foreach (var dto in request.OutletOrderTypes)
            {
                var ot = _mapper.Map<OutletOrderType>(dto);
                ot.OrderTypeId = entity.Id;
                ot.Sequence = (maxSequence.FirstOrDefault(x => x.Id == dto.OutletId)?.Max ?? 0) + 1;
                ot.CreateBy(_actor);
                _dbContext.OutletOrderTypes.Add(ot);
            }
        }


        protected override async Task<Result> Validate(CreateOrderTypeCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

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
