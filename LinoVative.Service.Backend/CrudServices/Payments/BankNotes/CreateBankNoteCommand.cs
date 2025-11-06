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
    public class CreateBankNoteCommand : BankNoteDto, IRequest<Result>
    {
    }

    public class CreateBankNoteHandlerService : SaveNewServiceBase<BankNote, CreateBankNoteCommand>, IRequestHandler<CreateBankNoteCommand, Result>
    {
        
        public CreateBankNoteHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }

        protected override async Task<Result> Validate(CreateBankNoteCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return result;
        }
    }
}
