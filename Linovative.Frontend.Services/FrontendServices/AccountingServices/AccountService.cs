using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.MasterData.Accountings;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices.AccountingServices
{
    public interface IAccountService : IReadOnlyService<AccountViewDto>, ICrudInterfaces
    {

    }

    public class AccountService : CrudServiceAbstract<AccountViewDto>, IAccountService
    {
        public AccountService(IHttpClientFactory httpFactory, ILogger<AccountService> logger) : base(httpFactory, logger, "Accounts")
        {
        }

    }
}
