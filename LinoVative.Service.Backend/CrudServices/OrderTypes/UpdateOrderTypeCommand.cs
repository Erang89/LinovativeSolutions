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
    public class UpdateOrderTypeCommand : OrderTypeDto, IRequest<Result>
    {
    }

    public class UpdateOrderTypeHandlerService : SaveUpdateServiceBase<OrderType, UpdateOrderTypeCommand>, IRequestHandler<UpdateOrderTypeCommand, Result>
    {
        public UpdateOrderTypeHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        public Task<Result> Handle(UpdateOrderTypeCommand request, CancellationToken ct) => base.SaveUpdate(request, ct);

        protected override async Task<Result> ValidateSaveUpdate(UpdateOrderTypeCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);
            
            // Your validations are here

            return result;
        }
    }
}
