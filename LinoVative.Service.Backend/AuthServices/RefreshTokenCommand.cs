using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Settings;
using LinoVative.Shared.Dto;
using Microsoft.Extensions.Options;

namespace LinoVative.Service.Backend.AuthServices
{
    public class RefreshTokenCommand : IRequest<Result>
    {
        public Guid? CompanyId { get; set; }
        public Guid? Token { get; set; }
        public string? RequestIPAddress { get; private set; }
        public void SetRequestIP(string? requestIP) => RequestIPAddress = requestIP;
    }

    public class RefreshTokenHandlerService : IRequestHandler<RefreshTokenCommand, Result>
    {
        private readonly IAppDbContext _dbCtx;
        private readonly IUserPrivilegeService _userPrevilege;
        private readonly IActor _actor;
        private readonly ILangueageService _lang;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenHandlerService(IAppDbContext dbCtx, IUserPrivilegeService userPri, IActor actor, ILangueageService lang, IOptions<AppSettings> appSettings)
        {
            _dbCtx = dbCtx;
            _userPrevilege = userPri;
            _actor = actor;
            _lang = lang;
            _lang.EnsureLoad(AvailableLanguageKeys.RefreshTokenCommand);
            _jwtSettings = appSettings.Value.JwtSettings;
        }

        public async Task<Result> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var validate = await Validate(request, ct);
            if(!validate) return validate;

            var user = _dbCtx.Users.FirstOrDefault(x => x.Id ==  _actor.UserId);
            var token = await JwtTokeProvider.Generate(user!, _jwtSettings, request.CompanyId, request.RequestIPAddress!, _dbCtx);

            return Result.OK(token);
        }

        async Task<Result> Validate(RefreshTokenCommand request, CancellationToken token)
        {
            var isCompanyIdExis = _userPrevilege.GetUserCompanyIds(_actor).Where(x => x == request.CompanyId).Any();
            if (!isCompanyIdExis)
                return Result.Failed(_lang[$"RefreshTokenCommand.CompanyId.NotFound"]);

            var isValidToken = _dbCtx.RefreshTokens.Where(x => x.Token! == request.Token && x.UserId == _actor.UserId && x.IPAddressLogin == request.RequestIPAddress).Any();
            if (!isValidToken)
                return Result.Failed(_lang["RefreshTokenCommand.Token.NotFound"]);

            await Task.CompletedTask;
            return Result.OK();
        }
    }
}
