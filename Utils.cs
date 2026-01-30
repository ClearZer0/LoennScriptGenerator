using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator;

public static class LoennScriptGeneratorUtils
{
    public static string ToLowerCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
            return input;
        return char.ToLower(input[0]) + input.Substring(1);
    }

    public static string ToUpperCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsUpper(input[0]))
            return input;
        return char.ToUpper(input[0]) + input.Substring(1);
    }
}
