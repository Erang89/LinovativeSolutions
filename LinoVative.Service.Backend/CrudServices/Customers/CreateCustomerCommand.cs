using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Customers.Shared;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Customers;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Customers;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Customers
{
    public class CreateCustomerCommand : CustomerInputDto, IRequest<Result>
    {
    }

    public class CreateCustomerHandlerService : SaveNewServiceBase<Customer, CreateCustomerCommand>
    {
        private readonly ICustomerValidationService _validator;
        
        public CreateCustomerHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, ICustomerValidationService validator) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
          _validator = validator;
        }

        public override Task BeforeSave(CreateCustomerCommand request, Customer entity, CancellationToken token)
        {
            var person = _mapper.Map<Core.People.Person>(request.Person!);
            _dbContext.People.Add(person);
            entity.PersonId = person.Id;

            foreach(var dto in request.Contacts)
            {
                var contact = _mapper.Map<CustomerContact>(dto);
                contact.CustomerId = entity.Id;
                _dbContext.CustomerContacts.Add(contact);
            }

            foreach(var dto in request.Address)
            {
                var address = _mapper.Map<CustomerAddress>(dto);
                address.CustomerId = entity.Id;
                _dbContext.CustomerAddress.Add(address);
            }

            return Task.CompletedTask;
        }

        protected override async Task<Result> Validate(CreateCustomerCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return await _validator.Validate(request);
        }
    }
}
