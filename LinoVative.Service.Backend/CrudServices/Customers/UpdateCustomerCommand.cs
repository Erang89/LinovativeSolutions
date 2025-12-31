using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Customers.Shared;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Customers;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Customers;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Customers
{
    public class UpdateCustomerCommand : CustomerInputDto, IRequest<Result>
    {
    }

    public class UpdateCustomerHandlerService : SaveUpdateServiceBase<Customer, UpdateCustomerCommand>
    {
        private readonly ICustomerValidationService _validator;

        public UpdateCustomerHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, ICustomerValidationService validator) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
            _validator = validator;
        }


        protected override async Task BeforeSaveUpdate(UpdateCustomerCommand request, Customer entity, CancellationToken token)
        {
            await MappPerson(request, entity);
            await MappAddress(request, entity);
            await MappContact(request, entity);
        }



        async Task MappContact(UpdateCustomerCommand request, Customer entity)
        {
            var contacts = await _dbContext.CustomerContacts.GetAll(_actor).Where(x => x.CustomerId == entity.Id).ToListAsync();
            var inputIds = request.Contacts.Select(x => x.Id).ToList();
            foreach(var conta in contacts.Where(x => !inputIds.Contains(x.Id)))
                conta.Delete(_actor);

            foreach(var contact in request.Contacts)
            {
                var existing = contacts.FirstOrDefault(x => x.Id == contact.Id);
                if(existing is not null)
                {
                    _mapper.Map(contact, existing);
                    if (_dbContext.GetEntityState(existing) == EntityState.Modified)
                        existing.ModifyBy(_actor);

                    continue;
                }

                var newContact = _mapper.Map<CustomerContact>(contact);
                newContact.CreateBy(_actor);
                newContact.CustomerId = entity.Id;
                _dbContext.CustomerContacts.Add(newContact);
            }
            
        }

        async Task MappAddress(UpdateCustomerCommand request, Customer entity)
        {
            var address = await _dbContext.CustomerAddress.GetAll(_actor).Where(x => x.CustomerId == entity.Id).ToListAsync();
            var inputIds = request.Address.Select(x => x.Id).ToList();
            foreach (var add in address.Where(x => !inputIds.Contains(x.Id)))
                add.Delete(_actor);

            foreach(var dto in request.Address)
            {
                var existing = address.FirstOrDefault(x => x.Id == dto.Id);
                if(existing is not null)
                {
                    _mapper.Map(dto, existing);
                    if (_dbContext.GetEntityState(existing) == EntityState.Modified)
                        existing.ModifyBy(_actor);
                    continue;
                }

                var newAdd = _mapper.Map<CustomerAddress>(dto);
                newAdd.CustomerId = entity.Id;
                _dbContext.CustomerAddress.Add(newAdd);
            }
        }



        async Task MappPerson(UpdateCustomerCommand request, Customer entity)
        {
            if(request.PersonId is null)
            {
                entity.Person = null;
                entity.PersonId = null;
                return;
            }

            var person = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == entity.PersonId);
            _mapper.Map(request.Person, person);
        }

        protected override async Task<Result> ValidateSaveUpdate(UpdateCustomerCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);
            if (!result) return result;

            return await _validator.Validate(request);
        }
    }
}
