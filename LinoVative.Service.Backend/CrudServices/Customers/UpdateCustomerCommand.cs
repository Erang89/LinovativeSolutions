using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Customers.Shared;
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
