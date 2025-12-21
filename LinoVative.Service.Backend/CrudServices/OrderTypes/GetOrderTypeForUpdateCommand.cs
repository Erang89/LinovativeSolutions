using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.OrderTypes;
using LinoVative.Shared.Dto.Outlets;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.OrderTypes
{
    public class GetOrderTypeForUpdateCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }

    public class GetOrderTypeForUpdateCommandHandlerService :  IRequestHandler<GetOrderTypeForUpdateCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IActor _actor;

        public GetOrderTypeForUpdateCommandHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _actor = actor;
        }


        public async Task<Result> Handle(GetOrderTypeForUpdateCommand request, CancellationToken ct)
        {
            var orderType = await _dbContext.OrderTypes.GetAll(_actor)
                .ProjectToType<OrderTypeInputDto>(_mapper.Config)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if (orderType == null) return Result.Failed($"No Data with ID: {request.Id}");

            orderType.OutletOrderTypes = await
                _dbContext.OutletOrderTypes.GetAll(_actor)
                .Where(x => x.OrderTypeId == request.Id)
                .ProjectToType<OutletOrderTypeDto>(_mapper.Config)
                .ToListAsync();
           
            return Result.OK(orderType);
        }

    }
}
