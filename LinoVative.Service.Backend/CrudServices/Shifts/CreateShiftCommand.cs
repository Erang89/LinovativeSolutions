using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Shifts;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Shifts;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Shifts
{
    public class CreateShiftCommand : ShiftDto, IRequest<Result>
    {
    }

    public class CreateShiftHandlerService : SaveNewServiceBase<Shift, CreateShiftCommand>, IRequestHandler<CreateShiftCommand, Result>
    {
        
        public CreateShiftHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }

        protected override async Task<Result> Validate(CreateShiftCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            var isNameExist = GetAll().Where(x => x.CompanyId ==  _actor.CompanyId && x.Name!.Contains(request.Name!)).Any();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);

            return result;
        }
    }
}
