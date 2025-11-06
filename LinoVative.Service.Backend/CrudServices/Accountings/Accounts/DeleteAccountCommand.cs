using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Accounts
{
    public class DeleteAccountCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteCOAGroupHandlerService : SaveDeleteServiceBase<Account, DeleteAccountCommand>, IRequestHandler<DeleteAccountCommand, Result>
    {
        public DeleteCOAGroupHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

    }
}
