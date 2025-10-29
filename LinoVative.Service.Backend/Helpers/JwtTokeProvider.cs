using IdentityModel;
using LinoVative.Service.Backend.AuthServices;
using LinoVative.Service.Backend.Constans;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Settings;
using LinoVative.Shared.Dto.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinoVative.Service.Backend.Helpers
{
    public static class JwtTokeProvider
    {
        public static async Task<LoginResponseDto> Generate(AppUser user, JwtSettings jwtSettings, Guid? companyId, string ipAddress, IAppDbContext dbCtx)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user!.Id.ToString()),
                new Claim(JwtClaimTypes.Email, user!.EmailAddress),
                new Claim(JwtClaimTypes.NickName, user!.NikName!),
                //new Claim(JwtClaimTypes.PreferredUserName, user.UserName!),
            };

            if (user.DefaultCompanyId is not null)
                claims.Add(new Claim(AppJwtClaims.CompanyId, user.DefaultCompanyId.Value.ToString()));


            //var roles = user.UserRoles;

            //foreach (var role in roles)
            //{
            //    if (string.IsNullOrEmpty(role?.AppRole?.Name))
            //        continue;

            //    claims.Add(new Claim(JwtClaimTypes.Role, role.AppRole.Name));
            //}

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(jwtSettings.TokenExpiryMinutes);


            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var refreshToken = await GetRefreshToken(ipAddress, user, dbCtx);

            return new LoginResponseDto() { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpiryUTCTime = expires, RefreshToken = refreshToken };

        }

        static async Task<Guid> GetRefreshToken(string ipAddress, AppUser user, IAppDbContext dbCtx)
        {
            var token = dbCtx.RefreshTokens.Where(x => x.UserId == user.Id && x.IPAddressLogin == ipAddress).FirstOrDefault();
            
            if (token is null)
            {
                token = new() { UserId = user.Id, IPAddressLogin = ipAddress};
                dbCtx.RefreshTokens.Add(token);                
            }

            var actor = new SystemAdministrator();
            token.Token = Guid.NewGuid();
            await dbCtx.SaveAsync(actor);

            return token.Token.Value;
        }
    }
}
