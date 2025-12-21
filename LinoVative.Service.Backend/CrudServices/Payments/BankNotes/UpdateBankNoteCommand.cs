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
    public class UpdateBankNoteCommand : BankNoteInputDto, IRequest<Result>
    {
    }

    public class UpdateBankNoteHandlerService : SaveUpdateServiceBase<BankNote, UpdateBankNoteCommand>
    {
        public UpdateBankNoteHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        protected override async Task BeforeSaveUpdate(UpdateBankNoteCommand request, BankNote entity, CancellationToken token)
        {
            var opm = await _dbContext.OutletBankNotes.GetAll(_actor).Where(x => x.BankNoteId == entity.Id).ToListAsync();
            var maxSequence = await _dbContext.OutletBankNotes
                .GetAll(_actor)
                .Where(x => x.Outlet!.CompanyId == _actor.CompanyId)
                .GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) })
                .ToListAsync();

            foreach (var dto in request.OutletBankNotes)
            {
                var existing = opm.FirstOrDefault(x => x.Id == dto.Id);
                if (existing is not null)
                {
                    _mapper.Map(dto, existing);
                    if (_dbContext.GetEntityState(existing) == EntityState.Modified)
                        existing.ModifyBy(_actor);
                    continue;
                }

                var obn = _mapper.Map<OutletBankNote>(dto);
                obn.BankNoteId = entity.Id;
                obn.Sequence = (maxSequence.FirstOrDefault(x => x.Id == dto.OutletId)?.Max ?? 0) + 1;
                obn.CreateBy(_actor);
                _dbContext.OutletBankNotes.Add(obn);
            }
        }

        protected override async Task<Result> ValidateSaveUpdate(UpdateBankNoteCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);

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
