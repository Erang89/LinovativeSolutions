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
    public class CreatePaymentMethodGroupCommand : PaymentMethodGroupDto, IRequest<Result>
    {
    }

    public class CreatePaymentMethodGroupHandlerService : SaveNewServiceBase<PaymentMethodGroup, CreatePaymentMethodGroupCommand>, IRequestHandler<CreatePaymentMethodGroupCommand, Result>
    {
        
        public CreatePaymentMethodGroupHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }

        public Task<Result> Handle(CreatePaymentMethodGroupCommand request, CancellationToken ct) => base.SaveNew(request, ct);


        protected override async Task<Result> Validate(CreatePaymentMethodGroupCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return result;
        }
    }
}
