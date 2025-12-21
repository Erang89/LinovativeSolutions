using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.MasterData.Payments;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IPaymentMethodService : IReadOnlyService<PaymentMethodViewDto>, ICrudInterfaces
    {
        public Task<Response<PaymentMethodUpdateDto>> GetForUpdate(Guid id, CancellationToken token);
    }

    public class PaymentMethodService : CrudServiceAbstract<PaymentMethodViewDto>, IPaymentMethodService
    {
        public PaymentMethodService(IHttpClientFactory httpFactory, ILogger<PaymentMethodService> logger) : base(httpFactory, logger, "PaymentMethods")
        {
        }


        public async Task<Response<PaymentMethodUpdateDto>> GetForUpdate(Guid id, CancellationToken token) =>
            await base.GetForUpdateByID<PaymentMethodUpdateDto>(id, token);
    }
}
