using Celeste.Mod.LoennScriptGenerator.LoennScripts;
using Celeste.Mod.LoennScriptGenerator.LuaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;
using static Celeste.Mod.LoennScriptGenerator.LoennScripts.ElementMetadata;

namespace Celeste.Mod.LoennScriptGenerator.LoennModels;

public static class FieldInformation
{
    public static LuaAssignment Create(EntityScriptMetadata metadata)
    {
        // <moduleName>.fieldInformation = {}
        var fieldInformation = new LuaTable();
        var assignment = new LuaAssignment(LuaComposer.SimplePrefix(metadata.Name, "fieldInformation"), fieldInformation);
        assignment.IsLocal = false;

        // fill data from metadata
        foreach (var element in metadata.Elements)
        {
            var elementTable = CreateElement(element);
            if (!elementTable.IsEmpty)
                fieldInformation[element.Name] = elementTable;
        }

        return assignment;
    }

    private static LuaTable CreateElement(ElementMetadata element)
    {
        var elementTable = new LuaTable();

        // fieldType
        string fieldType = element.Type switch
        {
            ElementType.Int => "integer",
            ElementType.Color => "color",
            ElementType.List => "list",
            _ => string.Empty
        };
        if (fieldType != string.Empty)
            elementTable["fieldType"] = fieldType;

        // min/max for int/float
        if (element.MinimumValue is { } min)
            elementTable["minimumValue"] = fieldType == "integer" ? new LuaInt(Convert.ToInt32(min)) : new LuaFloat(min);
        if (element.MaximumValue is { } max)
            elementTable["maximumValue"] = fieldType == "integer" ? new LuaInt(Convert.ToInt32(max)) : new LuaFloat(max);

        // options for enum
        // enum table generate by EntityScript, so just reference it here
        if (element.Type == ElementType.Enum)
        {
            elementTable["options"] = new LuaIdentifier(element.EnumOptionsName!);
            elementTable["editable"] = element.Editable ? LuaBoolean.True : LuaBoolean.False;
        }

        // color
        if (element.UseAlpha)
            elementTable["useAlpha"] = LuaBoolean.True;

        // list
        // min/max elements
        if (element.ListMinimumElements is { } listMin)
            elementTable["minimumElements"] = new LuaInt(listMin);
        if (element.ListMaximumElements is { } listMax)
            elementTable["maximumElements"] = new LuaInt(listMax);
        // separator
        if (element.ListElementSeparator is { } separator)
            elementTable["elementSeparator"] = new LuaString(separator);
        // recursive gererate elementOptions
        if (element.ListElementMetadata is { } listElementsMetadata)
            elementTable["elementOptions"] = CreateElement(listElementsMetadata);

        return elementTable;
    }
}
