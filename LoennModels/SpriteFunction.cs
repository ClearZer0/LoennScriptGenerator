using Celeste.Mod.LoennScriptGenerator.LuaModels;
using Celeste.Mod.LoennScriptGenerator.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LoennModels;

public class FakeLuaTable
{
    public class FakeVarible
    {
        public static readonly FakeVarible Instance = new FakeVarible();
        private FakeVarible() { }

        public static implicit operator int(FakeVarible v) => 0;
        public static implicit operator float(FakeVarible v) => 0f;
        public static implicit operator bool(FakeVarible v) => false;
        public static implicit operator string(FakeVarible v) => string.Empty;
    }

    public static FakeLuaTable Instance = new FakeLuaTable();
    private FakeLuaTable() { }

    public FakeVarible this[string key] => FakeVarible.Instance;
}

public static class SpriteFunction
{
    public static LuaFunction? Create(EntityScriptMetadata metadata)
    {
        var result = FakeExecute(metadata.Config.SpriteFunction);
        var spriteMetadata = result.metadata;
        var rawExpression = result.raw;
        if (rawExpression == null)
            return null;

        // function <module.Name>.sprite(room, entity)
        var spriteFunc = new LuaFunction($"{metadata.Name}.sprite", "room", "entity") { IsLocal = false};
        ConvertBlock(spriteFunc);
        return spriteFunc;

        void ConvertBlock(LuaBlock block)
        {

        }
    }

    private static (SpriteFunctionMetadata? metadata, string raw) FakeExecute(
        Action<FakeLuaTable, FakeLuaTable, SpriteFunctionMetadata>? spriteFunc,
        [CallerArgumentExpression("spriteFunc")] string _raw = "")
    {
        if (spriteFunc == null)
            return (null, string.Empty);

        var metadata = new SpriteFunctionMetadata();
        spriteFunc(FakeLuaTable.Instance, FakeLuaTable.Instance, metadata);
        return (metadata, _raw);
    }
}
