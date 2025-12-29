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
    public class UpdateCustomertCommand : CustomerInputDto, IRequest<Result>
    {
    }

    public class UpdateCustomerHandlerService : SaveUpdateServiceBase<Customer, UpdateCustomertCommand>
    {
        public UpdateCustomerHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        protected override async Task<Result> ValidateSaveUpdate(UpdateCustomertCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);

            return result;
        }
    }
}
