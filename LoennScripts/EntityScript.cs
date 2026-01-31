using Celeste.Mod.LoennScriptGenerator.LoennModels;
using Celeste.Mod.LoennScriptGenerator.LuaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LoennScripts;

public class EntityScript : LoennScript
{
    protected override string ExportPath => "Loenn/entities";

    public EntityScript(EntityScriptMetadata metadata)
        : base(metadata.Name)
    {
        // tode: auto load necessary requires

        // auto generate options for enum
        GenerateOptions(metadata);

        // <moduleName>.Name = <[CustomEntity].Name>
        var customEntityName = new LuaAssignment(LuaComposer.SimplePrefix(ModuleName, "name"), metadata.Config.CustomEntityName);
        customEntityName.IsLocal = false;
        Add(customEntityName);

        // placements
        Add(Placements.Create(metadata));
        AddNewLine();

        // fieldInformation
        Add(FieldInformation.Create(metadata));
    }

    private void GenerateOptions(EntityScriptMetadata metadata)
    {
        /*
        <EnumOptionsName> = 
        {
            enum1 = "enum1",
            enum2 = "enum2",
            ...
            enumN = "enumN"
        }
        */
        foreach (var option in metadata.Elements.Where(m => m.EnumOptions != null).DistinctBy(m => m.EnumOptionsName))
        {
            var optionTable = new LuaTable();
            var assignment = new LuaAssignment(option.EnumOptionsName!, optionTable);
            for (int i = 0; i < option.EnumOptions!.Count; i++)
                optionTable[option.EnumOptionDisplayNames![i]] = LoennScriptGeneratorUtils.LowerCamelCase(option.EnumOptions![i]);

            Add(assignment);
            AddNewLine();
        }
    }

    private void ExportLang(EntityScriptMetadata metadata)
    {
        string fullPath = Path.Combine(DirectoryPath, "Loenn/lang", "en_gb.lang");
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        // wip
        if (File.Exists(fullPath))
        {
            // entities.<[CustomEntity].Name>.placements.name.<[CustomEntity].Name>=<Description>
            // entities.<[CustomEntity].Name>.attributes.description.<element.Name>=<Description>
        }
    }
}
