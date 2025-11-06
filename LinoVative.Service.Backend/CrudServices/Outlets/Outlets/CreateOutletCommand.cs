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
    public class CreateOutletCommand : OutletDto, IRequest<Result>
    {
    }

    public class CreateOutletHandlerService : SaveNewServiceBase<Outlet, CreateOutletCommand>, IRequestHandler<CreateOutletCommand, Result>
    {
        
        public CreateOutletHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }

        public Task<Result> Handle(CreateOutletCommand request, CancellationToken ct) => base.Handle(request, ct);


        protected override async Task<Result> Validate(CreateOutletCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            var isNameExist = GetAll().Where(x => x.CompanyId ==  _actor.CompanyId && x.Name!.Contains(request.Name!)).Any();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return result;
        }
    }
}
