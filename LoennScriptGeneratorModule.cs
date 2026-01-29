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
        var path = Metadata.PathDirectory;
        Logger.Info("LoennScriptGenerator", $"LoennScriptGeneratorModule loaded from {path}");

        LoennScriptGeneratorAPI.Load(Metadata);
        var script = LoennScriptGeneratorAPI.GenerateFor<TestEntity>();

        Logger.Info("LoennScriptGenerator", $"\n{script}");
        script.Export(LoennScripts.ExportMode.GenerateNew);
    }

    public override void Unload()
    {
    }
}