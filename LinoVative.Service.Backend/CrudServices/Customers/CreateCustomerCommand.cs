using Linovative.Shared.Interface;
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
        
        public CreateCustomerHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }


        protected override async Task<Result> Validate(CreateCustomerCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return result;
        }
    }
}
