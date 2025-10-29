using IdentityModel;
using LinoVative.Service.Backend.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Settings;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Auth;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinoVative.Service.Backend.AuthServices
{
    public class LoginCommand : LoginDto, IRequest<Result>
    {

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

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, _user!.Id.ToString()),
                new Claim(JwtClaimTypes.Email, _user!.EmailAddress),
                new Claim(JwtClaimTypes.NickName, _user!.NikName!),
                //new Claim(JwtClaimTypes.PreferredUserName, user.UserName!),
                //new Claim(JwtClaimTypes.ClientId, clientId.ToString())
            };


            //var roles = user.UserRoles;

            //foreach (var role in roles)
            //{
            //    if (string.IsNullOrEmpty(role?.AppRole?.Name))
            //        continue;

            //    claims.Add(new Claim(JwtClaimTypes.Role, role.AppRole.Name));
            //}

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryMinutes);


            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var response = new { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpiryUtcTime = expires };

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
