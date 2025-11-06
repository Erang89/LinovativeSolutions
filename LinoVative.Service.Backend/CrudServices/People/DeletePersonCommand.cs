using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.People;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.People
{
    public class DeletePersonCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeletePersonHandlerService : SaveDeleteServiceBase<Person, DeletePersonCommand>, IRequestHandler<DeletePersonCommand, Result>
    {
        public DeletePersonHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

    }
}
