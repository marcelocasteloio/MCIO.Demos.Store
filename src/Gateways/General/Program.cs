using Asp.Versioning;
using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck;
using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck.Models;
using MCIO.Demos.Store.BuildingBlock.WebApi.PropertyNamingPolicies;
using MCIO.Demos.Store.BuildingBlock.WebApi.RouteTokenTransformer;
using MCIO.Demos.Store.Gateways.General.Config;
using MCIO.Demos.Store.Gateways.General.HealthCheck;
using MCIO.Demos.Store.Gateways.General.Services;
using MCIO.Demos.Store.Gateways.General.Services.Identity.V1;
using MCIO.Demos.Store.Gateways.General.Services.Identity.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.GrpcServices;
using MCIO.Observability.Abstractions;
using MCIO.Observability.OpenTelemetry;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using MCIO.Demos.Store.BuildingBlock.Grpc.DependencyInjection;
using MCIO.Demos.Store.Gateways.General.Services.Analytics.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Basket.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Calendar.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Catalog.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Customer.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Delivery.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Notification.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Order.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Payment.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Pricing.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Product.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Analytics.V1;
using MCIO.Demos.Store.Gateways.General.Services.Basket.V1;
using MCIO.Demos.Store.Gateways.General.Services.Calendar.V1;
using MCIO.Demos.Store.Gateways.General.Services.Catalog.V1;
using MCIO.Demos.Store.Gateways.General.Services.Customer.V1;
using MCIO.Demos.Store.Gateways.General.Services.Delivery.V1;
using MCIO.Demos.Store.Gateways.General.Services.Notification.V1;
using MCIO.Demos.Store.Gateways.General.Services.Order.V1;
using MCIO.Demos.Store.Gateways.General.Services.Payment.V1;
using MCIO.Demos.Store.Gateways.General.Services.Pricing.V1;
using MCIO.Demos.Store.Gateways.General.Services.Product.V1;

var builder = WebApplication.CreateBuilder(args);

var assemblyName = Assembly.GetExecutingAssembly().GetName();

var instanceId = Guid.NewGuid();
var applicationName = assemblyName.Name!;
var applicationVersion = assemblyName.Version?.ToString() ?? "no version";
var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

// Config
var config = builder.Configuration.Get<Config>()!;

#region [ Dependency Injection ]

// Configure Kestrel
builder.Services.Configure<KestrelServerOptions>(options =>
{
    // Http
    options.Listen(
        address: IPAddress.Any,
        port: config.Kestrel.HttpPort,
        options =>
        {
            options.Protocols = HttpProtocols.Http1;
        }
    );
    // Grpc
    options.Listen(
        address: IPAddress.Any,
        port: config.Kestrel.GrpcPort,
        options =>
        {
            options.Protocols = HttpProtocols.Http2;
        }
    );
});

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
    options.AddSecurityDefinition(
        name: "Bearer",
        securityScheme: new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        }
    );
    options.AddSecurityRequirement(
        securityRequirement: new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
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

// Services
builder.Services.AddScoped<IAnalyticsContextService, AnalyticsContextService>().AddHttpClient<AnalyticsContextService>();
builder.Services.AddScoped<IBasketContextService, BasketContextService>().AddHttpClient<BasketContextService>();
builder.Services.AddScoped<ICalendarContextService, CalendarContextService>().AddHttpClient<CalendarContextService>();
builder.Services.AddScoped<ICatalogContextService, CatalogContextService>().AddHttpClient<CatalogContextService>();
builder.Services.AddScoped<ICustomerContextService, CustomerContextService>().AddHttpClient<CustomerContextService>();
builder.Services.AddScoped<IDeliveryContextService, DeliveryContextService>().AddHttpClient<DeliveryContextService>();
builder.Services.AddScoped<IIdentityContextService, IdentityContextService>().AddHttpClient<IdentityContextService>();
builder.Services.AddScoped<INotificationContextService, NotificationContextService>().AddHttpClient<NotificationContextService>();
builder.Services.AddScoped<IOrderContextService, OrderContextService>().AddHttpClient<OrderContextService>();
builder.Services.AddScoped<IPaymentContextService, PaymentContextService>().AddHttpClient<PaymentContextService>();
builder.Services.AddScoped<IPricingContextService, PricingContextService>().AddHttpClient<PricingContextService>();
builder.Services.AddScoped<IProductContextService, ProductContextService>().AddHttpClient<ProductContextService>();

// Authorization
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuers = [config.Token.Issuer],

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Token.PrivateKey)),
            ValidateIssuerSigningKey = true,

            RequireAudience = true,
            ValidateAudience = true,
            ValidAudiences = config.Token.AudienceCollection ?? [],

            ValidateLifetime = true,
            RequireExpirationTime = true,

            RequireSignedTokens = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// GrpcServices
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = null;
    options.MaxSendMessageSize = null;
});

// GrpcServices Client
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Analytics.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.AnalyticsContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Basket.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.BasketContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Calendar.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.CalendarContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Catalog.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.CatalogContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Customer.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.CustomerContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Delivery.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.DeliveryContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Identity.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.IdentityContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Notification.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.NotificationContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Order.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.OrderContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Payment.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.PaymentContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Pricing.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.PricingContext
);
builder.Services.RegisterNamedGrpcClient<MCIO.Demos.Store.Product.WebApi.Protos.V1.PingService.PingServiceClient>(
    grpcServiceConfig: config.ExternalServices.GrpcServiceCollection.ProductContext
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

// GrpcServices
app.MapGrpcService<PingGrpcService>();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseAuthentication();
app.UseAuthorization();

#endregion [ Pipeline ]

#region [ Startup ]
_ = StartupService.TryStartupAsync(app.Services);
#endregion [ Startup ]

app.Run();
