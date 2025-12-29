using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Customers;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Customers
{
    public class DeleteCustomerCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteCustomerHandlerService : SaveDeleteServiceBase<Customer, DeleteCustomerCommand>, IRequestHandler<DeleteCustomerCommand, Result>
    {
        public DeleteCustomerHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }
    }
}
