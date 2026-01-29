using Celeste.Mod.LoennScriptGenerator.LuaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LoennScripts;

public enum ExportMode
{
    SkipIfExists,
    ForceOverride,
    GenerateNew
}

public abstract class LoennScript : LuaModule
{
    // set by LoennScriptGenerator, from EverestModule.Metadata.PathDirectory
    public static string DirectoryPath = string.Empty;
    protected abstract string ExportPath { get; }

    public LoennScript(string name)
        : base(name)
    {
    }

    public void Export(ExportMode mode)
    {
        string fullPath = Path.Combine(DirectoryPath, ExportPath, $"{ModuleName}.lua");
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        if (File.Exists(fullPath))
        {
            switch (mode)
            {
            case ExportMode.SkipIfExists:
                return;

            case ExportMode.ForceOverride:
                break;

            case ExportMode.GenerateNew:
                string dir = Path.GetDirectoryName(fullPath)!;
                string fileName = Path.GetFileNameWithoutExtension(fullPath);
                string extension = Path.GetExtension(fullPath);

                int index = 1;
                string newPath;

                do
                {
                    newPath = Path.Combine(dir, $"{fileName}_{index}{extension}");
                    index++;
                }
                while (File.Exists(newPath));

                fullPath = newPath;
                break;
            }
        }

        File.WriteAllText(fullPath, ToLua(), new UTF8Encoding(false));
    }
}
