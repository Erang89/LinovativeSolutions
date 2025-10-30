using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto.MasterData.Payments;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Payments.BankNotes
{
    public class GetAllBankNoteIQueryableCommand : IRequest<IQueryable<BankNoteDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllBankNoteQueryableHandlerService : QueryServiceBase<BankNote, GetAllBankNoteIQueryableCommand>, IRequestHandler<GetAllBankNoteIQueryableCommand, IQueryable<BankNoteDto>>
    {
        public GetAllBankNoteQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<BankNote> OnGetAllFilter(IQueryable<BankNote> query, GetAllBankNoteIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req);
        }

        public Task<IQueryable<BankNoteDto>> Handle(GetAllBankNoteIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).AsNoTracking().ProjectToType<BankNoteDto>(_mapper.Config));

    }
}
