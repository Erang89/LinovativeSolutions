using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.MasterData.Customers;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ICustomerService : IReadOnlyService<CustomerViewDto>, ICrudInterfaces
    {
        public Task<Response<CustomerInputDto>> GetForUpdate(Guid id, CancellationToken token);
    }

    public class CustomerService : CrudServiceAbstract<CustomerViewDto>, ICustomerService
    {
        public CustomerService(IHttpClientFactory httpFactory, ILogger<CustomerService> logger) : base(httpFactory, logger, "customers")
        {
        }

        public async Task<Response<CustomerInputDto>> GetForUpdate(Guid id, CancellationToken token) =>
            await base.GetForUpdateByID<CustomerInputDto>(id, token);
    }
}
