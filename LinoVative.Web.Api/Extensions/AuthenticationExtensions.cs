using IdentityModel;
using LinoVative.Service.Backend.Constans;
using LinoVative.Service.Core.Constants;
using LinoVative.Service.Core.Settings;
using LinoVative.Web.Api.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace LinoVative.Web.Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("AppSettings").Get<AppSettings>()!.JwtSettings;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(AppSchemeNames.CommonApiScheme, options =>
            {
                options.TokenValidationParameters = GenerateTokenValidatorParameter(jwtSettings!);
                options.Events = new JwtBearerEvents { OnTokenValidated = context => ValidateContext(context, AppSchemeNames.CommonApiScheme) };
            })
            .AddJwtBearer(AppSchemeNames.MainAPIScheme, options =>
            {
                options.TokenValidationParameters = GenerateTokenValidatorParameter(jwtSettings!);
                options.Events = new JwtBearerEvents { OnTokenValidated = context => ValidateContext(context, AppSchemeNames.MainAPIScheme) };
            })
            .AddJwtBearer(AppSchemeNames.ManagementAPI, options =>
            {
                options.TokenValidationParameters = GenerateTokenValidatorParameter(jwtSettings!);
                options.Events = new JwtBearerEvents { OnTokenValidated = context => ValidateContext(context, AppSchemeNames.ManagementAPI) };
            });

            services.ConfigureRole();
            return services;
        }

        private static void ConfigureRole(this IServiceCollection services)
        {
            // Write all the policies here
            services.AddAuthorization(options =>
            {
                //options.AddPolicy(ServicePolicyName.IntegrationPolicy, policy => policy.RequireClaim(JwtClaimTypes.Scope, LookupIds.ApplicationServices.ReservationAPI.ToString()));
                //foreach (var priviligeId in PrivilegeNames.IdMappings)
                //{
                //    // Booking Confirmation Policy
                //    if (priviligeId.Key == PrivilegeNames.Booking_Booking_Confirmation || priviligeId.Key == PrivilegeNames.Reports_Booking_Confirmation)
                //    {
                //        options.AddPolicy(priviligeId.Key, policy =>
                //            policy.RequireAssertion(context =>
                //                context.User.HasClaim(AdditionalClaimTypes.Priviliges, PrivilegeNames.Booking_Booking_Confirmation) ||
                //                context.User.HasClaim(AdditionalClaimTypes.Priviliges, PrivilegeNames.Reports_Booking_Confirmation)
                //           ));
                //        continue;
                //    }

                //    // Edit Booking
                //    if (priviligeId.Key == PrivilegeNames.Booking_Edit_Booking)
                //    {
                //        options.AddPolicy(priviligeId.Key, policy =>
                //           policy.RequireAssertion(context =>
                //               context.User.HasClaim(AdditionalClaimTypes.Priviliges, PrivilegeNames.Booking_Edit_Booking) ||
                //               context.User.HasClaim(AdditionalClaimTypes.Priviliges, PrivilegeNames.Booking_Edit_booking_Payment) ||
                //               context.User.HasClaim(AdditionalClaimTypes.Priviliges, PrivilegeNames.Booking_Edit_Booking_Ticket) ||
                //               context.User.HasClaim(AdditionalClaimTypes.Priviliges, PrivilegeNames.Booking_AuditTrail) ||
                //               context.User.HasClaim(AdditionalClaimTypes.Priviliges, PrivilegeNames.Booking_NewBooking)
                //          ));
                //        continue;
                //    }

                //    // Default Policy
                //    options.AddPolicy(priviligeId.Key, policy => policy.RequireClaim(AdditionalClaimTypes.Priviliges, priviligeId.Key));
                //}
            });
        }

        private static TokenValidationParameters GenerateTokenValidatorParameter(JwtSettings jwtSettings)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                RoleClaimType = "role"
            };
        }

        private static Task ValidateContext(TokenValidatedContext context, string schemeName)
        {
            var identity = context.Principal?.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var companyId = identity.FindAll(AppJwtClaims.CompanyId).Select(c => c.Value).FirstOrDefault();
                //var priviligeClaims = identity.FindAll(AdditionalClaimTypes.Priviliges).Select(c => c.Value).ToList();

                if(schemeName == AppSchemeNames.MainAPIScheme && companyId is null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Fail("Invalid identity. Token does not contain required scope claims.");
                    return Task.CompletedTask;
                }


                if (schemeName == AppSchemeNames.ManagementAPI)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Fail("Invalid identity. Management Scheme is not configured yet.");
                    return Task.CompletedTask;
                }

                //if (scopeClaims == null || scopeClaims.Length == 0 || !scopeClaims.Any(x => x.ToLower() == serviceId.ToString().ToLower()))
                //{
                //    context.Fail("Invalid identity. Token does not contain required scope claims.");
                //    return Task.CompletedTask;
                //}

                //foreach (var scope in scopeClaims)
                //{
                //    identity.AddClaim(new Claim(JwtClaimTypes.Scope, scope));
                //}

                //foreach (var pid in priviligeClaims)
                //{
                //    identity.AddClaim(new Claim(AdditionalClaimTypes.Priviliges, PrivilegeNames.GetStringId(Int32.Parse(pid))));
                //}

            }
            else
            {
                context.Fail("Invalid identity.");
            }

            return Task.CompletedTask;
        }

    }
}
