using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Payments.PaymentMethods
{
    public class DeletePaymentMethodCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeletePaymentMethodHandlerService : SaveDeleteServiceBase<PaymentMethod, DeletePaymentMethodCommand>, IRequestHandler<DeletePaymentMethodCommand, Result>
    {
        public DeletePaymentMethodHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

    }
}
