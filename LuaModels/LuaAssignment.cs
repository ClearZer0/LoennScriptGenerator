using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LuaModels;

public class LuaAssignment : LuaElement
{
    private readonly LuaComposer identifiers = new(", ");
    private readonly LuaComposer values = new(", ");

    public bool IsLocal = true;

    public LuaAssignment(LuaIdentifier identifier, LuaValue value) => Add(identifier, value);
    public void Add(LuaIdentifier identifier, LuaValue? value)
    {
        identifiers.Add(identifier);
        if (value != null)
            values.Add(value);
    }

    public override string ToLua(int indentLevel = 0)
    {
        var localStr = IsLocal ? "local " : string.Empty;
        return $"{GetIndent(indentLevel)}{localStr}{identifiers} = {values}\n";
    }
}
