using Celeste.Mod.LoennScriptGenerator.API;
using Celeste.Mod.LoennScriptGenerator.Entities;

namespace Celeste.Mod.LoennScriptGenerator;

# nullable disable

public sealed class LoennScriptGeneratorModule : EverestModule
{
    public static LoennScriptGeneratorModule Instance { get; private set; }

    public override void Load()
    {
        Instance = this;

        LoennScriptGeneratorAPI.Load(Metadata);
        //var script = LoennScriptGeneratorAPI.GenerateFor<TestEntity>();

        // Logger.Info("LoennScriptGenerator", $"\n{script}");
        //script.Export(LoennScripts.ExportMode.SkipIfExists);
    }

    public override void Unload()
    {
    }
}

public static class Cmd
{
    [Command("lsg_test", "")]
    public static void Test()
    {
        var script = LoennScriptGeneratorAPI.GenerateFor<TestEntity>();

        Logger.Info("LoennScriptGenerator", $"\n{script}");
        script.Export(LoennScripts.ExportMode.ForceOverride);
    }
}
