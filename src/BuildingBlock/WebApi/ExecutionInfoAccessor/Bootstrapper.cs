using MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor;
public static class Bootstrapper
{
    public static IServiceCollection AddExecutionInfoAccessor(
        this IServiceCollection services
    )
    {
        return services.AddScoped<IExecutionInfoAccessor>(serviceProvider =>
        {
            Guid? tenantCode = null;
            Guid? correlationId = null;
            string? user = null;
            string? acceptLanguage = null;
            string? origin = null;

            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            if (httpContextAccessor?.HttpContext is null)
                return new ExecutionInfoAccessor(correlationId, tenantCode, user, acceptLanguage, origin);

            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext.Request.Headers.TryGetValue(IExecutionInfoAccessor.TENANT_CODE_HEADER_KEY, out var tenantCodeHeaderValue))
                if (Guid.TryParse(tenantCodeHeaderValue.ToString(), out var tenantCodeParsed))
                    tenantCode = tenantCodeParsed;

            if (httpContext.Request.Headers.TryGetValue(IExecutionInfoAccessor.CORRELATION_ID_HEADER_KEY, out var correlationIdHeaderValue))
                if (Guid.TryParse(correlationIdHeaderValue.ToString(), out var correlationIdParsed))
                    correlationId = correlationIdParsed;

            if (httpContext.Request.Headers.TryGetValue(IExecutionInfoAccessor.USER_HEADER_KEY, out var userHeaderValue))
                user = userHeaderValue;

            acceptLanguage = httpContext.Request.Headers.TryGetValue(IExecutionInfoAccessor.ACCEPT_LANGUAGE_HEADER_KEY, out var acceptLanguageHeaderValue)
                ? (string?)acceptLanguageHeaderValue
                : "en-US";

            if (httpContext.Request.Headers.TryGetValue(IExecutionInfoAccessor.ORIGIN_HEADER_KEY, out var originHeaderValue))
                origin = originHeaderValue;
            else 
                origin = httpContext.Request.Headers.TryGetValue(IExecutionInfoAccessor.HOST_HEADER_KEY, out var hostHeaderValue) 
                    ? (string?)hostHeaderValue 
                    : IExecutionInfoAccessor.DEFAULT_ORIGIN_VALUE;

            return new ExecutionInfoAccessor(correlationId, tenantCode, user, acceptLanguage, origin);
        });
    }
}
