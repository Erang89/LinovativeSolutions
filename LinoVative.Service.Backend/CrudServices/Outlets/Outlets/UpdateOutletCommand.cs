using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Outlets.Outlets.Helpers;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Outlets;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Outlets
{
    public class UpdateOutletCommand : OutletDto, IRequest<Result>
    {
    }

    public class UpdateOutletHandlerService : SaveUpdateServiceBase<Outlet, UpdateOutletCommand>
    {
        private readonly IOutletInputValidationService _validator;

        public UpdateOutletHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, IOutletInputValidationService validator) :
            base(dbContext, actor, mapper, appCache, localizer)
        {
            _validator = validator;
        }

        protected override async Task BeforeSaveUpdate(UpdateOutletCommand request, Outlet entity, CancellationToken token)
        {
            await UpdateShift(request, entity);
            await UpdateBankNote(request, entity);
            await UpdatePaymentMethod(request, entity);
            await UpdateOrderType(request, entity);
            await UpdateItemGroup(request, entity);
            await UpdateItemCategory(request, entity);
            await UpdateItemException(request, entity);
        }


        async Task UpdateShift(UpdateOutletCommand request, Outlet outlet)
        {
            var outletShifts =  await _dbContext.OutletShifts.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ToListAsync();

            var sequence = 1;
            foreach (var dto in request.Shifts.OrderBy(x => x.Sequence))
            {
                var outletShift = outletShifts.FirstOrDefault(x => x.Id == dto.Id);
                var entityExisit = outletShift is not null;
                outletShift ??= _mapper.Map(dto, new OutletShift() { Id = dto.Id, OutletId = request.Id });
                outletShift.Sequence = sequence;
                sequence++;

                if (!entityExisit)
                {
                    outletShift.CreateBy(_actor);
                    _dbContext.OutletShifts.Add(outletShift);
                    continue;
                }

                _mapper.Map(dto, outletShift);
            }
        }


        async Task UpdateBankNote(UpdateOutletCommand request, Outlet outlet)
        {
            var outletBankNotes = await _dbContext.OutletBankNotes.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ToListAsync();

            var sequence = 1;
            foreach (var dto in request.BankNotes.OrderBy(x => x.Sequence))
            {
                var bankNotes = outletBankNotes.FirstOrDefault(x => x.Id == dto.Id);
                var isExist = bankNotes is not null;
                bankNotes ??= _mapper.Map(dto, new OutletBankNote() { Id = dto.Id, OutletId = request.Id });
                bankNotes.Sequence = sequence;
                sequence++;
                if (!isExist)
                {
                    bankNotes.CreateBy(_actor);
                    _dbContext.OutletBankNotes.Add(bankNotes);
                    continue;
                }
                _mapper.Map(dto, bankNotes);
            }
        }

        async Task UpdatePaymentMethod(UpdateOutletCommand request, Outlet outlet)
        {
            var outletPaymentMethods = await _dbContext.OutletPaymentMethods.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ToListAsync();

            var sequence = 1;
            foreach (var dto in request.PaymentMethods.OrderBy(x => x.Sequence))
            {
                var paymentMethod = outletPaymentMethods.FirstOrDefault(x => x.Id == dto.Id);
                var isExist = paymentMethod is not null;
                paymentMethod ??= _mapper.Map(dto, new OutletPaymentMethod() { Id = dto.Id, OutletId = request.Id });
                paymentMethod.Sequence = sequence;
                sequence++;
                if (!isExist)
                {
                    paymentMethod.CreateBy(_actor);
                    _dbContext.OutletPaymentMethods.Add(paymentMethod);
                    continue;
                }
                _mapper.Map(dto, paymentMethod);
            }
        }


        async Task UpdateOrderType(UpdateOutletCommand request, Outlet outlet)
        {
            var outletOrderTypes = await _dbContext.OutletOrderTypes.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ToListAsync();

            var sequence = 1;
            foreach (var dto in request.OrderTypes.OrderBy(x => x.Sequence))
            {
                var orderType = outletOrderTypes.FirstOrDefault(x => x.Id == dto.Id);
                var isExist = orderType is not null;
                orderType ??= _mapper.Map(dto, new OutletOrderType() { Id = dto.Id, OutletId = request.Id });
                orderType.Sequence = sequence;
                sequence++;
                if (!isExist)
                {
                    orderType.CreateBy(_actor);
                    _dbContext.OutletOrderTypes.Add(orderType);
                    continue;
                }
                _mapper.Map(dto, orderType);
            }



        }

        async Task UpdateItemGroup(UpdateOutletCommand request, Outlet outlet)
        {
            var outletItemGroups = await _dbContext.OutletItemGroups.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ToListAsync();

            var sequence = 1;
            foreach (var dto in request.ItemGroups.OrderBy(x => x.Sequence))
            {
                var group = outletItemGroups.FirstOrDefault(x => x.Id == dto.Id);
                var isExist = group is not null;
                group ??= _mapper.Map(dto, new OutletItemGroup() { Id = dto.Id, OutletId = request.Id });
                group.Sequence = sequence;
                sequence++;
                if (!isExist)
                {
                    group.CreateBy(_actor);
                    _dbContext.OutletItemGroups.Add(group);
                    continue;
                }
                _mapper.Map(dto, group);
            }
        }

        async Task UpdateItemCategory(UpdateOutletCommand request, Outlet outlet)
        {
            var outletItemCategories = await _dbContext.OutletItemCategories.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ToListAsync();

            var sequence = 1;
            foreach (var dto in request.ItemCategories.OrderBy(x => x.Sequence))
            {
                var category = outletItemCategories.FirstOrDefault(x => x.Id == dto.Id);
                var isExist = category is not null;
                category ??= _mapper.Map(dto, new OutletItemCategory() { Id = dto.Id, OutletId = request.Id });
                category.Sequence = sequence;
                sequence++;
                if (!isExist)
                {
                    category.CreateBy(_actor);
                    _dbContext.OutletItemCategories.Add(category);
                    continue;
                }
                _mapper.Map(dto, category);
            }
        }

        async Task UpdateItemException(UpdateOutletCommand request, Outlet outlet)
        {
            var outletItemExceptions = await _dbContext.OutletItemExceptionals.GetAll(_actor).Where(x => x.OutletId == outlet.Id).ToListAsync();

            foreach (var dto in request.OutletItemExceptionals)
            {
                var item = outletItemExceptions.FirstOrDefault(x => x.Id == dto.Id);
                var isExist = item is not null;
                item ??= _mapper.Map(dto, new OutletItemExceptional() { Id = dto.Id, OutletId = request.Id });
                if (!isExist)
                {
                    item.CreateBy(_actor);
                    _dbContext.OutletItemExceptionals.Add(item);
                    continue;
                }
                _mapper.Map(dto, item);
            }
        }

        protected override async Task<Result> ValidateSaveUpdate(UpdateOutletCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);
            if (!result) return result;

            return await _validator.Validate(request, token);
        }
    }
}
