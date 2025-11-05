using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.COAGroups
{
    public class DeleteCOAGroupCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteCOAGroupHandlerService : SaveDeleteServiceBase<COAGroup, DeleteCOAGroupCommand>, IRequestHandler<DeleteCOAGroupCommand, Result>
    {
        public DeleteCOAGroupHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

        public Task<Result> Handle(DeleteCOAGroupCommand request, CancellationToken ct) => base.Handle(request, ct);
    }
}
