using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.MasterData.Payments;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IPaymentMethodGroupService : IReadOnlyService<PaymentMethodGroupViewDto>, ICrudInterfaces
    {

    }

    public class PaymentMethodGroupService : CrudServiceAbstract<PaymentMethodGroupViewDto>, IPaymentMethodGroupService
    {
        public PaymentMethodGroupService(IHttpClientFactory httpFactory, ILogger<PaymentMethodGroupService> logger) : base(httpFactory, logger, "PaymentMethodGroups")
        {
        }

    }
}
