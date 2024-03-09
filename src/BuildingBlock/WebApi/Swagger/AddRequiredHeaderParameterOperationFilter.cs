using MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.Swagger;

public class AddRequiredHeaderParameterOperationFilter
    : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = IExecutionInfoAccessor.TENANT_CODE_HEADER_KEY,
            In = ParameterLocation.Header,
            Required = true
        });
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = IExecutionInfoAccessor.USER_HEADER_KEY,
            In = ParameterLocation.Header,
            Required = true
        });
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = IExecutionInfoAccessor.ACCEPT_LANGUAGE_HEADER_KEY,
            In = ParameterLocation.Header,
            Required = true
        });
    }
}
