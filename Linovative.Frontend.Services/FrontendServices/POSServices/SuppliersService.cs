using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.MasterData.Suppliers;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ISupplierService : IReadOnlyService<SupplierDto>, ICrudInterfaces
    {
        public Task<Response<SupplierInputDto>> GetForUpdate(Guid id, CancellationToken token);
    }

    public class SupplierService : CrudServiceAbstract<SupplierDto>, ISupplierService
    {
        public SupplierService(IHttpClientFactory httpFactory, ILogger<SupplierService> logger) : base(httpFactory, logger, "Suppliers")
        {
        }

        public async Task<Response<SupplierInputDto>> GetForUpdate(Guid id, CancellationToken token) =>
            await base.GetForUpdateByID<SupplierInputDto>(id, token);
    }
}
