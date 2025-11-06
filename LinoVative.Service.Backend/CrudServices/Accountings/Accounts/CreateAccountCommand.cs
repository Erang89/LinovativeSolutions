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
    public class CreateAccountCommand : AccountDto, IRequest<Result>
    {
    }

    public class CreateAccountHandlerService : SaveNewServiceBase<Account, CreateAccountCommand>, IRequestHandler<CreateAccountCommand, Result>
    {
        
        public CreateAccountHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }


        protected override async Task<Result> Validate(CreateAccountCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return result;
        }
    }
}
