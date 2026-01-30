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
        // tode: requires

        // todo: auto generate options for enum

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
}
