using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.MasterData.Payments;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IBankNoteService : IReadOnlyService<BankNoteDto>, ICrudInterfaces
    {
        public Task<Response<BankNoteInputDto>> GetForUpdate(Guid id, CancellationToken token);
    }

    public class BanoNoteService : CrudServiceAbstract<BankNoteDto>, IBankNoteService
    {
        public BanoNoteService(IHttpClientFactory httpFactory, ILogger<BanoNoteService> logger) : base(httpFactory, logger, "BankNotes")
        {
        }

        public async Task<Response<BankNoteInputDto>> GetForUpdate(Guid id, CancellationToken token) =>
            await base.GetForUpdateByID<BankNoteInputDto>(id, token);
    }
}
