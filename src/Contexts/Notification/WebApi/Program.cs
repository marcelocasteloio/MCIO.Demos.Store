using Asp.Versioning;
using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck;
using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck.Models;
using MCIO.Demos.Store.BuildingBlock.WebApi.PropertyNamingPolicies;
using MCIO.Demos.Store.BuildingBlock.WebApi.RouteTokenTransformer;
using MCIO.Demos.Store.Notification.WebApi.Config;
using MCIO.Demos.Store.Notification.WebApi.HealthCheck;
using MCIO.Demos.Store.Notification.WebApi.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var assemblyName = Assembly.GetExecutingAssembly().GetName();

var applicationName = assemblyName.Name!;
var applicationVersion = assemblyName.Version?.ToString() ?? "no version";

// Config
var config = builder.Configuration.Get<Config>()!;

#region [ Dependency Injection ]

// Health check
builder.Services
    .AddHealthChecks()
    .AddCheck<Startup>(config.HealthCheck.StartupPath, tags: new[] { HealthCheckBase.HEALTH_CHECK_STARTUP_TAG })
    .AddCheck<Readiness>(config.HealthCheck.ReadinessPath, tags: new[] { HealthCheckBase.HEALTH_CHECK_READINESS_TAG })
    .AddCheck<Liveness>(config.HealthCheck.LivenessPath, tags: new[] { HealthCheckBase.HEALTH_CHECK_LIVENESS_TAG });

// Controllers
builder.Services
    .AddControllers(options => {
        options.Conventions.Add(
            new RouteTokenTransformerConvention(new SlugifyParameterTransformer())    
        );
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = new KebabCaseNamingPolicy();
    });

// API versioning
builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(majorVersion: 1, minorVersion: 0);
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = applicationName,
        Description = "",
        Contact = new OpenApiContact()
        {
            Name = "Marcelo Castelo Branco",
            Email = "contato@marcelocastelo.io",
            Url = new Uri("https://www.linkedin.com/company/marcelocasteloio")
        }
    });
});

// Routing
builder.Services.AddRouting(options => options.LowercaseUrls = true);

#endregion [ Dependency Injection ]

var app = builder.Build();

#region [ Pipeline ]

// Health check
app.MapHealthChecks(
    pattern: config.HealthCheck.StartupPath,
    options: new HealthCheckOptions
    {
        AllowCachingResponses = false,
        ResponseWriter = ReportWriter.WriteReport,
        Predicate = healthCheck => healthCheck.Tags.Contains(HealthCheckBase.HEALTH_CHECK_STARTUP_TAG)
    }
);
app.MapHealthChecks(
    pattern: config.HealthCheck.ReadinessPath,
    options: new HealthCheckOptions
    {
        AllowCachingResponses = false,
        ResponseWriter = ReportWriter.WriteReport,
        Predicate = healthCheck => healthCheck.Tags.Contains(HealthCheckBase.HEALTH_CHECK_READINESS_TAG)
    }
);
app.MapHealthChecks(
    pattern: config.HealthCheck.LivenessPath,
    options: new HealthCheckOptions
    {
        AllowCachingResponses = false,
        ResponseWriter = ReportWriter.WriteReport,
        Predicate = healthCheck => healthCheck.Tags.Contains(HealthCheckBase.HEALTH_CHECK_LIVENESS_TAG)
    }
);

// Controllers
app.MapControllers();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

#endregion [ Pipeline ]

#region [ Startup ]
_ = StartupService.TryStartupAsync(app.Services);
#endregion [ Startup ]

app.Run();