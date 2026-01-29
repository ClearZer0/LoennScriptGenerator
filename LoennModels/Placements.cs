using Celeste.Mod.LoennScriptGenerator.LoennScripts;
using Celeste.Mod.LoennScriptGenerator.LuaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Celeste.Mod.LoennScriptGenerator.LoennScripts.ElementMetadata;

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

    private static LuaValue ConvertValue(ElementMetadata metadata, object value) => metadata.Type switch
    {
        ElementType.Int => new LuaInt(Convert.ToInt32(value)),
        ElementType.Float => new LuaFloat(Convert.ToSingle(value)),
        ElementType.Bool => new LuaBoolean((bool)value),
        ElementType.List => ConvertValue(metadata.ListElementsMetadata!, value),

        // string = "value"
        // enum = "value" (generate from actual enum type)
        // color = "value" (ffffff etc generate from default value)
        _ => new LuaString((string)value)
    };
}
