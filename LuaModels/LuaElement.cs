using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace Celeste.Mod.LoennScriptGenerator.LuaModels;

public abstract class LuaElement
{
    public abstract string ToLua(int indentLevel = 0);
    public override string ToString() => ToLua();
    protected string GetIndent(int indentLevel) => new string(' ', indentLevel * 4);
}

public class LuaComment : LuaElement
{
    private string comment;
    public LuaComment(string comment) => this.comment = comment;
    public override string ToLua(int indentLevel = 0) => $"{GetIndent(indentLevel)}-- {comment}\n";
}

public class LuaNewLine : LuaElement
{
    public readonly static LuaNewLine Instance = new LuaNewLine();
    private LuaNewLine() { }
    public override string ToLua(int indentLevel = 0) => "\n";
}
