using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace LinoVative.Web.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Private API doc
                c.SwaggerDoc("linovative-private-v1", new OpenApiInfo
                {
                    Title = "Linovative - Private V1.0",
                    Description = "These endpoints are only available to Linovative internal projects",
                    Version = "v1"
                });

                // Public API doc
                c.SwaggerDoc("linovative-public-v1", new OpenApiInfo
                {
                    Title = "Linovative Public V1.0",
                    Version = "v1",
                    Description = "These API endpoints are available to public<br/>"
                });

                // Map DateOnly -> string
                c.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date",
                    Example = new OpenApiString("2025-08-26")
                });

                // Route-based filtering: private vs public docs
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var path = apiDesc.RelativePath?.TrimStart('/').ToLowerInvariant();

                    if (string.IsNullOrEmpty(path) || path.Contains("/$count"))
                        return false;

                    if (docName == "linovative-public-v1")
                        return path.StartsWith("public/api/v1");

                    if (docName == "linovative-private-v1")
                        return path.StartsWith("api/v1");

                    return false;
                });

                // Define Bearer scheme globally
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid JWT token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                // ❌ Don't add global security requirement
                // ✅ Only add requirement for private endpoints
                c.OperationFilter<PrivateAuthOperationFilter>();

                // XML comments (if generated)
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }
            });

            services.Configure<SwaggerUIOptions>(options =>
            {
                options.DocExpansion(DocExpansion.None);
                options.SwaggerEndpoint("/swagger/linovative-private-v1/swagger.json", "Linovative - Private V1.0");
                options.SwaggerEndpoint("/swagger/linovative-public-v1/swagger.json", "Linovative - Public V1.0");

                options.InjectStylesheet("/swagger-ui/custom.css");
                options.InjectJavascript("/swagger-ui/custom.js");
            });

            return services;
        }
    }

    /// <summary>
    /// Applies Bearer token requirement only to private API endpoints (/api/v1/...).
    /// </summary>
    internal class PrivateAuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var path = context.ApiDescription.RelativePath?
                .TrimStart('/')
                .ToLowerInvariant();

            if (string.IsNullOrEmpty(path))
                return;

            if (path.StartsWith("api/v1")) // private endpoints only
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }
                    ] = Array.Empty<string>()
                });
            }
        }
    }
}
