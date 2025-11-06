using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.OrderTypes;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.OrderTypes;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.OrderTypes
{
    public class CreateOrderTypeCommand : OrderTypeDto, IRequest<Result>
    {
    }

    public class CreateOrderTypeHandlerService : SaveNewServiceBase<OrderType, CreateOrderTypeCommand>, IRequestHandler<CreateOrderTypeCommand, Result>
    {
        
        public CreateOrderTypeHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }


        protected override async Task<Result> Validate(CreateOrderTypeCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return result;
        }
    }
}
