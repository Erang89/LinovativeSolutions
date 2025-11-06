using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.Outlets;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ITableService : IReadOnlyService<OutletTableViewDto>, ICrudInterfaces
    {

    }

    public class TableService : CrudServiceAbstract<OutletTableViewDto>, ITableService
    {
        public TableService(IHttpClientFactory httpFactory, ILogger<TableService> logger) : base(httpFactory, logger, "OutletTables")
        {
        }

    }
}
