using Celeste.Mod.LoennScriptGenerator.LuaModels;
using Celeste.Mod.LoennScriptGenerator.Metadatas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LoennModels;

public static class Placements
{
    public static LuaAssignment Create(EntityScriptMetadata metadata)
    {
        // <moduleName>.placements = {}
        var placements = new LuaTable();
        var assignment = new LuaAssignment(LuaComposer.SimplePrefix(metadata.Name, "placements"), placements);
        assignment.IsLocal = false;

        // name = <moduleName>
        placements["name"] = metadata.Name;

        // data = {}
        var data = new LuaTable();
        placements["data"] = data;

        // fill data from metadata
        foreach (var element in metadata.Elements)
        {
            if (element.DefaultValue == null)
                continue;

            var value = ConvertValue(element, element.DefaultValue);
            data[element.Name] = value;
        }

        return assignment;
    }

    private static LuaValue ConvertValue(ElementMetadata metadata, object value)
    {
        if (value is not IList list)
            return ConvertAtom(metadata, value);

        // list
        var separator = metadata.ListElementSeparator ?? ",";
        var composer = new LuaComposer(separator);
        foreach (var element in list.Cast<object>())
        {
            var result = ConvertValue(metadata.ListElementMetadata!, element);
            if (result is LuaString s)
                result = s.ToIdentifier();
            composer.Add(result);
        }
        return new LuaString(composer.ToLua());

        static LuaValue ConvertAtom(ElementMetadata metadata, object value) => value switch
        {
            int => new LuaInt(Convert.ToInt32(value)),
            float => new LuaFloat(Convert.ToSingle(value)),
            bool => new LuaBoolean((bool)value),
            Enum e => new LuaString(LoennScriptGeneratorUtils.LowerCamelCase(e.ToString())),
            Color c => metadata.UseAlpha ? new LuaString($"{c.R:X2}{c.G:X2}{c.B:X2}{c.A:X2}") : new LuaString($"{c.R:X2}{c.G:X2}{c.B:X2}"),
            _ => new LuaString((string)value)
        };
    }
}
