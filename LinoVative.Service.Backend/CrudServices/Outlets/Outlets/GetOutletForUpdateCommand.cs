using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Outlets;
using LinoVative.Shared.Dto.Outlets;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Outlets
{
    public class GetOutletForUpdateCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }

    public class GetOutletForUpdateCommandHandlerService :  IRequestHandler<GetOutletForUpdateCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IActor _actor;

        public GetOutletForUpdateCommandHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _actor = actor;
        }


        public async Task<Result> Handle(GetOutletForUpdateCommand request, CancellationToken ct)
        {
            var outlet = await _dbContext.Outlets.ProjectToType<OutletViewDto>(_mapper.Config).FirstOrDefaultAsync(x => x.Id == request.Id);
            if (outlet == null) return Result.Failed($"No Data with ID: {request.Id}");

            await LoadShift(outlet);
            await LoadBankNotes(outlet);
            await LoadPaymentMethods(outlet);
            await LoadorderTypes(outlet);
            await LoadItemExeptional(outlet);
            await LoadItemGroup(outlet);
            await LoadItemCategory(outlet);

            return Result.OK(outlet);
        }

        async Task LoadShift(OutletViewDto outlet) => outlet.Shifts = await _dbContext.OutletShifts.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ProjectToType<OutletShiftDto>(_mapper.Config).ToListAsync();
        async Task LoadBankNotes(OutletViewDto outlet) => outlet.BankNotes = await _dbContext.OutletBankNotes.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ProjectToType<OutletBankNoteDto>(_mapper.Config).ToListAsync();
        async Task LoadPaymentMethods(OutletViewDto outlet) => outlet.PaymentMethods = await _dbContext.OutletPaymentMethods.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ProjectToType<OutletPaymentMethodDto>(_mapper.Config).ToListAsync();
        async Task LoadorderTypes(OutletViewDto outlet) => outlet.OrderTypes = await _dbContext.OutletOrderTypes.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ProjectToType<OutletOrderTypeDto>(_mapper.Config).ToListAsync();
        async Task LoadItemExeptional(OutletViewDto outlet) => outlet.OutletItemExceptionals = await _dbContext.OutletItemExceptionals.GetAll(_actor).Where(x => x.EntityId == outlet.Id).ProjectToType<OutletItemExceptionalDto>(_mapper.Config).ToListAsync();
        async Task LoadItemGroup(OutletViewDto outlet) => outlet.ItemGroups = await _dbContext.OutletItemGroups.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ProjectToType<OutletItemGroupDto>(_mapper.Config).ToListAsync();
        async Task LoadItemCategory(OutletViewDto outlet) => outlet.ItemCategories = await _dbContext.OutletItemCategories.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ProjectToType<OutletItemCategoryDto>(_mapper.Config).ToListAsync();

    }
}
