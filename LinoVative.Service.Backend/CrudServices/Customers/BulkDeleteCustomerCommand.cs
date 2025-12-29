using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Customers;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Customers
{
    public class BulkDeleteCustomerCommand : IRequest<Result>, IBulkDeleteDto
    {
        public List<Guid> Ids { get; set; } = new();
    }

    public class BulkDeleteCustomerHandlerService : SaveDeleteServiceBase<Customer, BulkDeleteCustomerCommand>, IRequestHandler<BulkDeleteCustomerCommand, Result>
    {
        public BulkDeleteCustomerHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }
    }
}
