using Asp.Versioning;
using MCIO.Demos.Store.Payment.WebApi.HealthCheck;
using MCIO.Demos.Store.Payment.WebApi.Services;
using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck;
using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck.Models;
using MCIO.Demos.Store.BuildingBlock.WebApi.PropertyNamingPolicies;
using MCIO.Demos.Store.BuildingBlock.WebApi.RouteTokenTransformer;
using MCIO.Observability.Abstractions;
using MCIO.Observability.OpenTelemetry;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using OpenTelemetry.Logs;
using MCIO.Demos.Store.Payment.WebApi.Config;

var builder = WebApplication.CreateBuilder(args);

var assemblyName = Assembly.GetExecutingAssembly().GetName();

var instanceId = Guid.NewGuid();
var applicationName = assemblyName.Name!;
var applicationVersion = assemblyName.Version?.ToString() ?? "no version";

// Config
var config = builder.Configuration.Get<Config>()!;

#region [ Dependency Injection ]

// Config
builder.Services.AddSingleton(config);

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

// Observability
builder.Services.AddSingleton(serviceProvider => new ActivitySource(applicationName));
builder.Services.AddSingleton<ITraceManager, TraceManager>();

builder.Services.AddSingleton(serviceProvider => new Meter(applicationName, applicationVersion));
builder.Services.AddSingleton<IMetricsManager>(serviceProvider => new MetricsManager(serviceProvider.GetRequiredService<Meter>()));

// Observability - OpenTelemetry
var batchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
{
    MaxQueueSize = config.OpenTelemetry.MaxQueueSize,
    ExporterTimeoutMilliseconds = config.OpenTelemetry.ExporterTimeoutMilliseconds,
    MaxExportBatchSize = config.OpenTelemetry.MaxExportBatchSize,
    ScheduledDelayMilliseconds = config.OpenTelemetry.ScheduledDelayMilliseconds
};

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(builder => {
        builder.AddService(
            serviceName: applicationName,
            serviceNamespace: applicationName,
            serviceVersion: applicationVersion,
            autoGenerateServiceInstanceId: false,
            serviceInstanceId: instanceId.ToString()
        );
    })
    .WithTracing(builder => builder
        .AddHttpClientInstrumentation(options => { })
        .AddAspNetCoreInstrumentation(options => { })
        .AddSource(applicationName)
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(config.OpenTelemetry.GrpcCollectorReceiverUrl);
            options.BatchExportProcessorOptions = batchExportProcessorOptions;
        })
    )
    .WithMetrics(builder => builder
        .AddMeter(applicationName)
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(config.OpenTelemetry.GrpcCollectorReceiverUrl);
            options.BatchExportProcessorOptions = batchExportProcessorOptions;
        })
    );

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
