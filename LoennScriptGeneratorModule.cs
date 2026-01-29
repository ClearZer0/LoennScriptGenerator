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
    }

    public override void Unload()
    {
    }
}