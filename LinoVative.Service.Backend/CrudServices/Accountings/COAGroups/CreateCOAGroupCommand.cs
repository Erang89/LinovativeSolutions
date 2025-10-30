using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Accountings;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.COAGroups
{
    public class CreateCOAGroupCommand : COAGroupDto, IRequest<Result>
    {
    }

    public class CreateCOAGroupHandlerService : SaveNewServiceBase<COAGroup, CreateCOAGroupCommand>, IRequestHandler<CreateCOAGroupCommand, Result>
    {
        
        public CreateCOAGroupHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }

        public Task<Result> Handle(CreateCOAGroupCommand request, CancellationToken ct) => base.SaveNew(request, ct);


        protected override async Task<Result> Validate(CreateCOAGroupCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return result;
        }
    }
}
