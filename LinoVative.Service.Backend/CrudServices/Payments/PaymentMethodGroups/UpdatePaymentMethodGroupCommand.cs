using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Payments;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Payments.PaymentMethodGroups
{
    public class UpdatePaymentMethodGroupCommand : PaymentMethodGroupDto, IRequest<Result>
    {
    }

    public class UpdatePaymentMethodGroupHandlerService : SaveUpdateServiceBase<PaymentMethodGroup, UpdatePaymentMethodGroupCommand>, IRequestHandler<UpdatePaymentMethodGroupCommand, Result>
    {
        public UpdatePaymentMethodGroupHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        public Task<Result> Handle(UpdatePaymentMethodGroupCommand request, CancellationToken ct) => base.SaveUpdate(request, ct);

        protected override async Task<Result> ValidateSaveUpdate(UpdatePaymentMethodGroupCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);
            
            // Your validations are here

            return result;
        }
    }
}
