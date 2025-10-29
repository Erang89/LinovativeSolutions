using LinoVative.Service.Backend.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Settings;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Auth;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace LinoVative.Service.Backend.AuthServices
{
    public class LoginCommand : LoginDto, IRequest<Result>
    {
        public string? RequestIPAddress { get; private set; }
        public void SetRequestIP(string? requestIP) => RequestIPAddress = requestIP;
    }

    public class LoginHandlerService : IRequestHandler<LoginCommand, Result>
    {
        readonly IAppDbContext _dbContext;
        readonly IStringLocalizer _loc;
        AppUser? _user;
        private readonly JwtSettings _jwtSettings;

        public LoginHandlerService(IOptions<AppSettings> appSettings, IAppDbContext dbContext, IStringLocalizer loc)
        {
            _dbContext = dbContext;
            _loc = loc;
            _jwtSettings = appSettings.Value.JwtSettings;
        }


        public async Task<Result> Handle(LoginCommand request, CancellationToken ct)
        {
            var validate = await Validate(request, ct);
            if (!validate) return validate;

            var response = await JwtTokeProvider.Generate(_user!, _jwtSettings, _user!.DefaultCompanyId, request.RequestIPAddress!, _dbContext);

            return Result.OK(response);
        }


        async Task<Result> Validate(LoginCommand request, CancellationToken token)
        {
            _user = _dbContext.Users.Where(x => x.IsActive && x.EmailAddress == request.UserName).FirstOrDefault();
            if (_user is null) return Result.Failed(_loc["LoginDto.Validate.LoginFailed"]);

            var isValidPassword = PasswordHelper.VerifyUserPassword(request.Password!, _user.Password, _user.Id);
            if(!isValidPassword) return Result.Failed(_loc["LoginDto.Validate.LoginFailed"]);

            await Task.CompletedTask;
            return Result.OK();
        }
    }
}
