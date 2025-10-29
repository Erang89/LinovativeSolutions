using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Outlets
{
    public class DeleteOutletCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteOutletHandlerService : SaveDeleteServiceBase<Outlet, DeleteOutletCommand>, IRequestHandler<DeleteOutletCommand, Result>
    {
        public DeleteOutletHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

        public Task<Result> Handle(DeleteOutletCommand request, CancellationToken ct) => base.SaveDelete(request, ct);
    }
}
