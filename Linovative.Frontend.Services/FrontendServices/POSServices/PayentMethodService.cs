using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.MasterData.Payments;
using LinoVative.Shared.Dto.MasterData.Shifts;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IPaymentMethodService : IReadOnlyService<PaymentMethodViewDto>, ICrudInterfaces
    {

    }

    public class PaymentMethodService : CrudServiceAbstract<PaymentMethodViewDto>, IPaymentMethodService
    {
        public PaymentMethodService(IHttpClientFactory httpFactory, ILogger<PaymentMethodService> logger) : base(httpFactory, logger, "PaymentMethods")
        {
        }

    }
}
