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
    public class UpdatePersonCommand : PersonDto, IRequest<Result>
    {
    }

    public class UpdatePersonHandlerService : SaveUpdateServiceBase<Person, UpdatePersonCommand>, IRequestHandler<UpdatePersonCommand, Result>
    {
        public UpdatePersonHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        public Task<Result> Handle(UpdatePersonCommand request, CancellationToken ct) => base.SaveUpdate(request, ct);

        protected override async Task<Result> ValidateSaveUpdate(UpdatePersonCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);
            
            // Your validations are here

            return result;
        }
    }
}
