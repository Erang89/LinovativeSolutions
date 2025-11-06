using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Payments.PaymentMethodGroups
{
    public class DeletePaymentMethodGroupCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeletePaymentMethodGroupHandlerService : SaveDeleteServiceBase<PaymentMethodGroup, DeletePaymentMethodGroupCommand>, IRequestHandler<DeletePaymentMethodGroupCommand, Result>
    {
        public DeletePaymentMethodGroupHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

    }
}
