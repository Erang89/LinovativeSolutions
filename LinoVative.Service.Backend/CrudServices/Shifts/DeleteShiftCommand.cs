using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Shifts;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Shifts
{
    public class DeleteShiftCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteShiftHandlerService : SaveDeleteServiceBase<Shift, DeleteShiftCommand>, IRequestHandler<DeleteShiftCommand, Result>
    {
        public DeleteShiftHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

        public Task<Result> Handle(DeleteShiftCommand request, CancellationToken ct) => base.SaveDelete(request, ct);
    }
}
