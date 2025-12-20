using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Outlets.Outlets.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Outlets;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Outlets
{
    public class CreateOutletCommand : OutletDto, IRequest<Result>
    {
    }

    public class CreateOutletHandlerService : SaveNewServiceBase<Outlet, CreateOutletCommand>
    {

        private readonly IOutletInputValidationService _validator;

        public CreateOutletHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer,  IOutletInputValidationService validator) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
            _validator = validator;
        }

        protected override Task<Outlet> OnMapping(CreateOutletCommand request, CancellationToken ct)
        {
            var outlet = base.OnMapping(request, ct);

            // Handle Shift
            var sequence = 1;
            foreach (var dto in request.Shifts.OrderBy(x => x.Sequence))
            {
                var outletShift = _mapper.Map(dto, new OutletShift() { Id = dto.Id, OutletId = request.Id, Sequence = sequence});
                outletShift.CreateBy(_actor);
                _dbContext.OutletShifts.Add(outletShift);
                sequence++;
            }

            // Handle Bank Note
            sequence = 1;
            foreach (var dto in request.BankNotes.OrderBy(x => x.Sequence))
            {
                var bankNotes = _mapper.Map(dto, new OutletBankNote() { Id = dto.Id, OutletId = request.Id, Sequence = sequence});
                bankNotes.CreateBy(_actor);
                _dbContext.OutletBankNotes.Add(bankNotes);
                sequence++;
            }


            // Handle Payment Methods
            sequence = 1;
            foreach (var dto in request.PaymentMethods.OrderBy(x => x.Sequence))
            {
                var paymentMethod = _mapper.Map(dto, new OutletPaymentMethod() { Id = dto.Id, OutletId = request.Id, Sequence = sequence });
                paymentMethod.CreateBy(_actor);
                _dbContext.OutletPaymentMethods.Add(paymentMethod);
                sequence++;
            }


            // Handle order type
            sequence = 1;
            foreach (var dto in request.OrderTypes.OrderBy(x => x.Sequence))
            {
                var orderType = _mapper.Map(dto, new OutletOrderType() { Id = dto.Id, OutletId = request.Id, Sequence = sequence });
                orderType.CreateBy(_actor);
                _dbContext.OutletOrderTypes.Add(orderType);
                sequence++;
            }


            // Handle Item Group
            sequence = 1;
            foreach (var dto in request.ItemGroups.OrderBy(x => x.Sequence))
            {
                var group = _mapper.Map(dto, new OutletItemGroup() { Id = dto.Id, OutletId = request.Id, Sequence = sequence });
                group.CreateBy(_actor);
                _dbContext.OutletItemGroups.Add(group);
                sequence++;
            }

            // Handle Item Category
            sequence = 1;
            foreach (var dto in request.ItemCategories.OrderBy(x => x.Sequence))
            {
                var category = _mapper.Map(dto, new OutletItemCategory() { Id = dto.Id, OutletId = request.Id, Sequence = sequence });
                category.CreateBy(_actor);
                _dbContext.OutletItemCategories.Add(category);
                sequence++;
            }

            // Handle Item Exceptions
            foreach(var dto in request.OutletItemExceptionals)
            {
                var item = _mapper.Map(dto, new OutletItemExceptional() { Id = dto.Id, OutletId = request.Id });
                item.CreateBy(_actor);
                _dbContext.OutletItemExceptionals.Add(item);
            }

            return outlet;
        }

        protected override async Task<Result> Validate(CreateOutletCommand request, CancellationToken token)
        {
            var validate = await base.Validate(request, token);
            if (!validate) return validate;

            return await _validator.Validate(request, token);
        }
    }
}
