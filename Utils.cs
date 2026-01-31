using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator;

public static class LoennScriptGeneratorUtils
{
    public static string LowerCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
            return input;
        return char.ToLower(input[0]) + input.Substring(1);
    }

    public static string UpperCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsUpper(input[0]))
            return input;
        return char.ToUpper(input[0]) + input.Substring(1);
    }

    public static string SplitCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        return Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
    }
}
