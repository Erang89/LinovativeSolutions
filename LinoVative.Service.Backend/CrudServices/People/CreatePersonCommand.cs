using Linovative.Dto.MasterData.People;
using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.People;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.People
{
    public class CreatePersonCommand : PersonDto, IRequest<Result>
    {
    }

    public class CreatePersonHandlerService : SaveNewServiceBase<Person, CreatePersonCommand>, IRequestHandler<CreatePersonCommand, Result>
    {
        
        public CreatePersonHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }

        protected override async Task<Result> Validate(CreatePersonCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return result;
        }
    }
}
