using ExchangeRates.Application.Interfaces;
using ExchangeRates.Application.Services;
using ExchangeRates.Domain.Interfaces;
using ExchangeRates.Infrastructure.Data;
using ExchangeRates.Infrastructure.Services;
using ExchangeRates.Presentation.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Polly;
using Presentation.Dto;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add User Secrets and other configurations
builder.Configuration
    .AddUserSecrets<Program>(); // Add User Secrets support

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80); // Bind to port 80
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Exchange Rates API",
        Version = "v1",
        Description = "An API for managing foreign exchange rates.",
    });

    c.EnableAnnotations(); // Enable annotations for Swagger
    c.ExampleFilters();    // Enable examples
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<ExchangeRateCreateDtoExample>();

// Add database context and configure EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Register application services
builder.Services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
builder.Services.AddScoped<IExternalExchangeRateProvider, ExternalExchangeRateProvider>();
builder.Services.AddHttpClient<IExternalExchangeRateProvider, ExternalExchangeRateProvider>(client =>
{
    client.BaseAddress = new Uri("https://www.alphavantage.co/");
}).AddTransientHttpErrorPolicy(policy =>
    policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(300)));


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rates API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure database is created and apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
