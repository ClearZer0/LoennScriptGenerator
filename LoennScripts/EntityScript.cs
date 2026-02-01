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
    private EntityScriptMetadata metadata;

    public EntityScript(EntityScriptMetadata metadata)
        : base(metadata.Name)
    {
        this.metadata = metadata;

        // tode: auto load necessary requires

        // auto generate options for enum
        GenerateOptions(metadata);

        // <moduleName>.Name = <[CustomEntity].Name>
        var customEntityName = new LuaAssignment(LuaComposer.SimplePrefix(ModuleName, "name"), metadata.Config.__CustomEntityName);
        customEntityName.IsLocal = false;
        Add(customEntityName);

        // placements
        Add(Placements.Create(metadata));
        AddNewLine();

        // fieldInformation
        Add(FieldInformation.Create(metadata));
        AddNewLine();

        // fieldOrder
        Add(FieldOrder.Create(metadata));
    }

    public override void Export(ExportMode mode)
    {
        base.Export(mode);

        // wip
        // support ExportMode(?

        // ExportLang(metadata);
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

        var localizations = new List<string>();
        var customEntityName = metadata.Config.__CustomEntityName;
        var entityClassName = LoennScriptGeneratorUtils.SplitCamelCase(metadata.ClassName);
        var sectionHeader = $"# {LoennScriptGeneratorUtils.SplitCamelCase(entityClassName)}";

        /*
        # <[CustomEntity].Name>

        localizations

        # othetEntityName
        */
        localizations.Add(sectionHeader);
        localizations.Add(string.Empty);
        // entities.<[CustomEntity].Name>.placements.name.<moduleName>=<Description>
        localizations.Add($"entities.{customEntityName}.placements.name.{ModuleName}={entityClassName}");
        // entities.<[CustomEntity].Name>.attributes.description.<element.Name>=<Description>
        var attrDescs = metadata.Elements.Where(e => e.Description != string.Empty).Select(e => $"entities.{customEntityName}.attributes.description.{e.Name}={e.Description}");
        localizations.AddRange(attrDescs);
        localizations.Add(string.Empty);

        var fileLines = File.Exists(fullPath) ? File.ReadAllLines(fullPath).ToList() : new List<string>();
        int startIndex = fileLines.FindIndex(l => l.Trim() == sectionHeader);

        // found
        if (startIndex != -1)
        {
            // search # othetEntityName or EOF
            int endIndex = -1;
            if (startIndex + 1 < fileLines.Count)
                endIndex = fileLines.FindIndex(startIndex + 1, l => l.Trim().StartsWith("#"));
            if (endIndex == -1)
                endIndex = fileLines.Count;

            // replace section
            fileLines.RemoveRange(startIndex, endIndex - startIndex);
            fileLines.InsertRange(startIndex, localizations);
        }
        // not found
        else
        {
            if (fileLines.Count > 0 && !string.IsNullOrWhiteSpace(fileLines.Last()))
                fileLines.Add(string.Empty);

            // append to last of lang
            fileLines.AddRange(localizations);
        }

        File.WriteAllLines(fullPath, fileLines, new UTF8Encoding(false));
    }
}
