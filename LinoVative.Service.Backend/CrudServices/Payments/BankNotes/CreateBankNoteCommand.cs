using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Payments;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Payments.BankNotes
{

    public class CreateBankNoteCommand : BankNoteInputDto, IRequest<Result>
    {
    }

    public class CreateBankNoteHandlerService : SaveNewServiceBase<BankNote, CreateBankNoteCommand>
    {
        
        public CreateBankNoteHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }


        public override async Task  BeforeSave(CreateBankNoteCommand request, BankNote entity, CancellationToken token)
        {
            var maxSequence = await _dbContext.OutletBankNotes
                .GetAll(_actor)
                .Where(x => x.Outlet!.CompanyId == _actor.CompanyId)
                .GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) })
                .ToListAsync();

            foreach (var os in request.OutletBankNotes)
            {
                var newOs = _mapper.Map<OutletBankNote>(os);
                newOs.BankNoteId = entity.Id;
                newOs.CreateBy(_actor);
                newOs.Sequence = (maxSequence.FirstOrDefault(x => x.Id == os.OutletId)?.Max ?? 0) + 1;
                _dbContext.OutletBankNotes.Add(newOs);
            }
        }


        protected override async Task<Result> Validate(CreateBankNoteCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            var anyDuplicateOutlet = request.OutletBankNotes.GroupBy(x => x.OutletId).Any(x => x.Count() > 1);
            if (anyDuplicateOutlet)
                return Result.Failed("Duplicate Outlet ID is not allowed");

            var outletIds = request.OutletBankNotes.Select(x => x.OutletId).Distinct();
            var outletIdsCount = await _dbContext.Outlets.CountAsync(x => outletIds.Contains(x.Id));
            if (outletIds.Count() != outletIdsCount)
                return Result.Failed("Some outlet ID are not in the system");

            return result;
        }
    }
}
