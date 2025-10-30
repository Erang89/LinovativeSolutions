using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.OrderTypes;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.OrderTypes
{
    public class DeleteOrderTypeCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteOrderTypeHandlerService : SaveDeleteServiceBase<OrderType, DeleteOrderTypeCommand>, IRequestHandler<DeleteOrderTypeCommand, Result>
    {
        public DeleteOrderTypeHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

        public Task<Result> Handle(DeleteOrderTypeCommand request, CancellationToken ct) => base.SaveDelete(request, ct);
    }
}
