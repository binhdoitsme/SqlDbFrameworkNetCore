using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlDbFrameworkNetCore.Helpers
{
    public static class StringToolkit
    {
        public static string PascalToUnderscore(string input)
        {
            var output = string.Join("_",
                    Regex.Replace(input,
                    "([A-Z])",
                    " $1",
                    RegexOptions.Compiled)
                    .Trim()
                    .ToLower()
                    .Split(" "));
            return output;
        }

        public static string UnderscoreToPascal(string input)
        {
            var tmpStr = input.ToLower().Replace("_", " ");
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            tmpStr = info.ToTitleCase(tmpStr).Replace(" ", string.Empty);
            return tmpStr;
        }

        // extension method to remove quote characters from string
        public static string TrimQuotes(this string input)
        {
            string output = input.Trim()
                            .Replace("'", "")
                            .Replace("`", "")
                            .Replace("\"", "");
            return output;
        }
    }
}
