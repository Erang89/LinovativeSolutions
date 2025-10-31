using LinoVative.Service.Backend.DatabaseService;
using Mapster;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using LinoVative.Service.Backend;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core;
using LinoVative.Service.Backend.MediaTRs;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Backend.LocalizerServices;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using LinoVative.Service.Backend.Configurations;
using MapsterMapper;
using LinoVative.Web.Api.Extensions;
using LinoVative.Web.Api.Middlewares;
using Microsoft.AspNetCore.Mvc;
using LinoVative.Service.Core.Settings;
using LinoVative.Web.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddConfigureSwagger();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddLogging();
builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = null;
        options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableTimeSpanConverter());
    })
    .ConfigurePrivateOData(builder.Services)
    .ConfigurePublicOData(builder.Services);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new TimeSpanConverter());
    options.SerializerOptions.Converters.Add(new NullableTimeSpanConverter());
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(kvp => kvp.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,                 // "Name", "Email", etc.
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var problem = new
            {
                status = StatusCodes.Status400BadRequest,
                //title = "One or more validation errors occurred.",
                errors
            };

            return new BadRequestObjectResult(problem);
        };
    });


// Register Localizers
builder.Services.AddJsonLocalization(opt =>
{
    opt.BasePath = Path.Combine(builder.Environment.ContentRootPath, "Resources");
});
var supportedCultures = new[] { "en", "id" }
    .Select(c => new CultureInfo(c))
    .ToList();


// Register Services
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(Program), typeof(RegisterBackendService), typeof(RegisterCoreService))
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Register Mediator
builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.Scan(s => s
    .FromAssembliesOf(typeof(Program), typeof(RegisterBackendService), typeof(RegisterCoreService))
    .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));


// Variables
var services = builder.Services;
var configuration = builder.Configuration;


// Configure DbContext
var connectionString = configuration["ConnectionStrings:DefaultConnection"];
services.AddDbContext<IAppDbContext, AppDbContext>((options) =>
    options.UseSqlServer(connectionString)
);


// Configure Mapper
// Register Mapster mapper
TypeAdapterConfig.GlobalSettings.Scan(typeof(MapsterConfigs).Assembly);
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ODataMiddleware>();
app.UseStaticFiles();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        // 1) Prefer JWT claim "lang"
        new ClaimsRequestCultureProvider { ClaimType = "lang" },

        // 2) Fallback to query/header if no claim
        // new QueryStringRequestCultureProvider(),  // optional (?culture=id)
        new AcceptLanguageHeaderRequestCultureProvider()
    }
});

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "areas",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
//});
app.MapControllers();


app.Run();
