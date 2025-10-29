using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Outlets;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Outlets
{
    public class UpdateOutletCommand : OutletDto, IRequest<Result>
    {
    }

    public class UpdateOutletHandlerService : SaveUpdateServiceBase<Outlet, UpdateOutletCommand>, IRequestHandler<UpdateOutletCommand, Result>
    {
        public UpdateOutletHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        public Task<Result> Handle(UpdateOutletCommand request, CancellationToken ct) => base.SaveUpdate(request, ct);

        protected override async Task<Result> ValidateSaveUpdate(UpdateOutletCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);

            var isNameExist = GetAll().Where(x => x.Name!.Contains(request.Name!) && x.Id != request.Id).Any();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return result;
        }
    }
}
