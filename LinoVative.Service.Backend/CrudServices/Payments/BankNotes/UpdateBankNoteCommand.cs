using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Payments;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Payments.BankNotes
{
    public class UpdateBankNoteCommand : BankNoteDto, IRequest<Result>
    {
    }

    public class UpdateBankNoteHandlerService : SaveUpdateServiceBase<BankNote, UpdateBankNoteCommand>, IRequestHandler<UpdateBankNoteCommand, Result>
    {
        public UpdateBankNoteHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        public Task<Result> Handle(UpdateBankNoteCommand request, CancellationToken ct) => base.SaveUpdate(request, ct);

        protected override async Task<Result> ValidateSaveUpdate(UpdateBankNoteCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);
            
            // Your validations are here

            return result;
        }
    }
}
