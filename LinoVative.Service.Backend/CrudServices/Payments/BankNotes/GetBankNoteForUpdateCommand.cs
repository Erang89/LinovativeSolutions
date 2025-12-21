using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Payments;
using LinoVative.Shared.Dto.Outlets;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Payments.BankNotes
{
    public class GetBankNoteForUpdateCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }

    public class GetBankNoteForUpdateCommandHandlerService :  IRequestHandler<GetBankNoteForUpdateCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IActor _actor;

        public GetBankNoteForUpdateCommandHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _actor = actor;
        }


        public async Task<Result> Handle(GetBankNoteForUpdateCommand request, CancellationToken ct)
        {
            var paymentMethod = await _dbContext.BankNotes.GetAll(_actor)
                .ProjectToType<BankNoteInputDto>(_mapper.Config)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if (paymentMethod == null) return Result.Failed($"No Data with ID: {request.Id}");

            paymentMethod.OutletBankNotes = await
                _dbContext.OutletBankNotes.GetAll(_actor)
                .Where(x => x.BankNoteId == request.Id)
                .ProjectToType<OutletBankNoteDto>(_mapper.Config)
                .ToListAsync();
           
            return Result.OK(paymentMethod);
        }

    }
}
