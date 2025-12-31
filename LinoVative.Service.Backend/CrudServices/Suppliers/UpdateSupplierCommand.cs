using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Suppliers.Shared;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Suppliers;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Suppliers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Suppliers
{
    public class UpdateSupplierCommand : SupplierInputDto, IRequest<Result>
    {
    }

    public class UpdateSupplierService : SaveUpdateServiceBase<Supplier, UpdateSupplierCommand>
    {
        private readonly ISupplierValidatorService _validator;

        public UpdateSupplierService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, ISupplierValidatorService validator) :
            base(dbContext, actor, mapper, appCache, localizer)
        {
            _validator = validator;
        }

        protected override async Task BeforeSaveUpdate(UpdateSupplierCommand request, Supplier entity, CancellationToken token)
        {
            await MappingAddress(request, entity);
            await MappingContact(request, entity);
        }


        async Task MappingAddress(UpdateSupplierCommand request, Supplier entity)
        {
            var address = await _dbContext.SupplierAddress.Where(x => x.SupplierId == request.Id).ToListAsync();
            var addressIds = request.Addresses.Select(x => x.Id).ToList();
            foreach (var ad in address.Where(x => !addressIds.Contains(x.Id)))
                ad.Delete(_actor);

            foreach (var dto in request.Addresses)
            {
                var exisiting = address.FirstOrDefault(x => x.Id == dto.Id);
                if (exisiting is not null)
                {
                    _mapper.Map(dto, exisiting);
                    exisiting.SupplierId = entity.Id;
                    continue;
                }

                var newAddress = _mapper.Map<SupplierAddress>(dto);
                newAddress.SupplierId = entity.Id;
                _dbContext.SupplierAddress.Add(newAddress);
            }

        }

        async Task MappingContact(UpdateSupplierCommand request, Supplier entity)
        {
            var contacts = await _dbContext.SupplierContact.Where(x => x.SupplierId == request.Id).ToListAsync();
            var contactIds = request.Contacts.Select(x => x.Id).ToList();
            foreach (var contact in contacts.Where(x => !contactIds.Contains(x.Id)))
                contact.Delete(_actor);

            foreach (var dto in request.Contacts)
            {
                var exisiting = contacts.FirstOrDefault(x => x.Id == dto.Id);
                if (exisiting is not null)
                {
                    _mapper.Map(dto, exisiting);
                    exisiting.SupplierId = entity.Id;
                    continue;
                }

                var newContact = _mapper.Map<SupplierContact>(dto);
                newContact.SupplierId = entity.Id;
                _dbContext.SupplierContact.Add(newContact);
            }

        }

        protected override async Task<Result> ValidateSaveUpdate(UpdateSupplierCommand request, CancellationToken token)
        {
            var validate = await base.ValidateSaveUpdate(request, token);
            if (!validate) return validate;

            return await _validator.Validate(request);
        }
    }
}
