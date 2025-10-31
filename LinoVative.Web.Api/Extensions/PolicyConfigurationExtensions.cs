namespace LinoVative.Web.Api.Extensions
{
    public static class PolicyConfigurationExtensions
    {
        public const string CorsPolicyName = "AllowConfiguredOrigins";

        public static IServiceCollection AddCorsPolicyConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            var allowedOrigins = configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();
            var allowAll = configuration.GetSection("CorsSettings:AlloweAllOrigins").Get<bool>();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, policy =>
                {
                    var corsBuilder = allowAll ? policy.WithOrigins("*") : policy.WithOrigins(allowedOrigins!);

                    corsBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
