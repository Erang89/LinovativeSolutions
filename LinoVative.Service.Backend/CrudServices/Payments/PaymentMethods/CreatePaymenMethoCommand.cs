using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Payments;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Payments.PaymentMethods
{
    public class CreatePaymenMethoCommand : PaymentMethodDto, IRequest<Result>
    {
    }

    public class CreatePaymentMethodHandlerService : SaveNewServiceBase<PaymentMethod, CreatePaymenMethoCommand>, IRequestHandler<CreatePaymenMethoCommand, Result>
    {
        
        public CreatePaymentMethodHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }

        public Task<Result> Handle(CreatePaymenMethoCommand request, CancellationToken ct) => base.SaveNew(request, ct);


        protected override async Task<Result> Validate(CreatePaymenMethoCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return result;
        }
    }
}
