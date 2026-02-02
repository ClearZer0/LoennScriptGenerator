using Celeste.Mod.LoennScriptGenerator.LuaModels;
using Celeste.Mod.LoennScriptGenerator.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LoennModels;

public static class FieldOrder
{
    public static LuaAssignment Create(EntityScriptMetadata metadata)
    {
        // <moduleName>.fieldOrder = {}
        var fieldOrder = new LuaArray();
        var assignment = new LuaAssignment(LuaComposer.SimplePrefix(metadata.Name, "fieldOrder"), fieldOrder);
        assignment.IsLocal = false;
        fieldOrder.Flatten = false;

        // display x y width height first
        fieldOrder.Add(new LuaComposer(", ", "x", "y", "width", "height"));

        var orders = metadata.Elements.Where(e => e.FieldOrder != null).OrderBy(e => e.FieldOrder);
        foreach (var order in orders)
            fieldOrder.Add(order.Name);

        return assignment;
    }
}
