using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Warehoses;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Warehouses
{
    public class DeleteWarehouseCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteWarehouseHandlerService : SaveDeleteServiceBase<Warehouse, DeleteWarehouseCommand>, IRequestHandler<DeleteWarehouseCommand, Result>
    {
        public DeleteWarehouseHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

        public Task<Result> Handle(DeleteWarehouseCommand request, CancellationToken ct) => base.SaveDelete(request, ct);
    }
}
