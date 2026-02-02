using System;
using System.Collections;
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

public class LuaFragment : LuaElement
{
    private List<LuaElement> lines = new();
    public int Count => lines.Count;

    public void AddNewLine() => Add(LuaNewLine.Instance);
    public void Add(LuaElement line) => lines.Add(line);
    public void Add(IEnumerable<LuaElement> lines) => this.lines.AddRange(lines);
    public void Add(params LuaElement[] lines) => this.lines.AddRange(lines);

    public override string ToLua(int indentLevel = 0)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var line in lines)
            sb.Append(line.ToLua());
        return sb.ToString();
    }
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
