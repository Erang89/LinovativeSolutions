using LinoVative.Service.Backend.DatabaseService;
using Mapster;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using LinoVative.Service.Backend;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core;
using LinoVative.Service.Backend.MediaTRs;
using LinoVative.Service.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddLogging();

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
TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddMapster();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllers();


app.Run();
