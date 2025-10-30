using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Payments.BankNotes
{
    public class DeleteBankNoteCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteBankNoteHandlerService : SaveDeleteServiceBase<BankNote, DeleteBankNoteCommand>, IRequestHandler<DeleteBankNoteCommand, Result>
    {
        public DeleteBankNoteHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

        public Task<Result> Handle(DeleteBankNoteCommand request, CancellationToken ct) => base.SaveDelete(request, ct);
    }
}
