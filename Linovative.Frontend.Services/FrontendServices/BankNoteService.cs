using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.MasterData.Payments;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IBankNoteService : IReadOnlyService<BankNoteDto>, ICrudInterfaces
    {

    }

    public class BanoNoteService : CrudServiceAbstract<BankNoteDto>, IBankNoteService
    {
        public BanoNoteService(IHttpClientFactory httpFactory, ILogger<BanoNoteService> logger) : base(httpFactory, logger, "BankNotes")
        {
        }

    }
}
