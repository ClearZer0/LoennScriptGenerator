using Celeste.Mod.LoennScriptGenerator.LoennScripts;
using Celeste.Mod.LoennScriptGenerator.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.API;

public static class LoennScriptGeneratorAPI
{
    public static void Load(EverestModuleMetadata metadata) => LoennScript.DirectoryPath = metadata.PathDirectory;
    public static EntityScript GenerateFor<TEntity>() where TEntity : Entity => new EntityScript(EntityScriptMetadata.Create<TEntity>());
}
