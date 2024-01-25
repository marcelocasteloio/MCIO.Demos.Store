using Microsoft.AspNetCore.Routing;
using MCIO.Demos.Store.BuildingBlock.Core.ExtensionMethods;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.RouteTokenTransformer;
public sealed class SlugifyParameterTransformer 
    : IOutboundParameterTransformer
{
    // Public Methods
    public string? TransformOutbound(object? value)
    {
        if (value is null) 
            return null;

        return value?.ToString()?.ToKebabCase();
    }
}
