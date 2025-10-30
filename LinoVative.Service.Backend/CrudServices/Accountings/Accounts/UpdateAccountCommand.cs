using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Accountings;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Accounts
{
    public class UpdateAccountCommand : AccountDto, IRequest<Result>
    {
    }

    public class UpdateAccountHandlerService : SaveUpdateServiceBase<Account, UpdateAccountCommand>, IRequestHandler<UpdateAccountCommand, Result>
    {
        public UpdateAccountHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        public Task<Result> Handle(UpdateAccountCommand request, CancellationToken ct) => base.SaveUpdate(request, ct);

        protected override async Task<Result> ValidateSaveUpdate(UpdateAccountCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);
            
            // Your validations are here

            return result;
        }
    }
}
