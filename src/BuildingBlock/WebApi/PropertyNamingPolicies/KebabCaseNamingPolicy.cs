using System.Text.Json;
using MCIO.Demos.Store.BuildingBlock.Core.ExtensionMethods;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.PropertyNamingPolicies;
public class KebabCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name!.ToKebabCase()!;
    }
}
