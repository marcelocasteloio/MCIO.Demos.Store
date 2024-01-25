using System.Text;

namespace MCIO.Demos.Store.BuildingBlock.Core.ExtensionMethods;
public static class StringExtensionMethods
{
    // Constants
    public const char KEBAB_CASE_SEPARATOR = '-';

    // Public Methods
    public static string? ToKebabCase(this string? input)
    {
        if (input is null)
            return null;

        var kebabCase = new StringBuilder();
        var lastCharWasLowerCase = false;

        for (var i = 0; i < input.Length; i++)
        {
            var character = input[i];

            if (char.IsUpper(character))
            {
                if (lastCharWasLowerCase)
                    kebabCase.Append(KEBAB_CASE_SEPARATOR);

                kebabCase.Append(char.ToLower(character));
                lastCharWasLowerCase = false;
            }
            else
            {
                kebabCase.Append(character);
                lastCharWasLowerCase = true;
            }
        }

        return kebabCase.ToString();
    }
}
