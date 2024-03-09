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
            string? user = null;
            string? acceptLanguage = null;

            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            if (httpContextAccessor?.HttpContext is null)
                return new ExecutionInfoAccessor(tenantCode, user, acceptLanguage);

            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext.Request.Headers.TryGetValue(IExecutionInfoAccessor.TENANT_CODE_HEADER_KEY, out var tenantCodeHeaderValue))
                if (Guid.TryParse(tenantCodeHeaderValue.ToString(), out var tenantCodeParsed))
                    tenantCode = tenantCodeParsed;

            if (httpContext.Request.Headers.TryGetValue(IExecutionInfoAccessor.USER_HEADER_KEY, out var userHeaderValue))
                user = userHeaderValue;

            if (httpContext.Request.Headers.TryGetValue(IExecutionInfoAccessor.ACCEPT_LANGUAGE_HEADER_KEY, out var acceptLanguageHeaderValue))
                acceptLanguage = acceptLanguageHeaderValue;

            return new ExecutionInfoAccessor(tenantCode, user, acceptLanguage);
        });
    }
}
