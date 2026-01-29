using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace Celeste.Mod.LoennScriptGenerator.LuaModels;

public class LuaFunctionCall : LuaValue
{
    private readonly LuaIdentifier functionName;
    private readonly LuaComposer arguments = new(", ");

    public LuaFunctionCall(LuaIdentifier functionName, params LuaValue[] args)
    {
        this.functionName = functionName;
        AddArguments(args);
    }

    public void AddArgument(LuaValue arg) => arguments.Add(arg);
    public void AddArguments(IEnumerable<LuaValue> args) => arguments.Add(args);
    public void AddArguments(params LuaValue[] args) => arguments.Add(args);

    public override string ToLua(int indentLevel = 0) => $"{functionName}({arguments})";
}

public class LuaAnonymousFunction : LuaValue
{
    private readonly LuaComposer arguments = new(", ");
    private readonly List<LuaElement> lines = new();

    public LuaAnonymousFunction(params LuaIdentifier[] args) => arguments.Add(args);

    public void AddArgument(LuaIdentifier arg) => arguments.Add(arg);
    public void AddArguments(IEnumerable<LuaIdentifier> args) => arguments.Add(args);
    public void AddArguments(params LuaIdentifier[] args) => arguments.Add(args);

    public void AddLine(LuaElement line) => lines.Add(line);
    public void AddLines(IEnumerable<LuaElement> lines) => this.lines.AddRange(lines);
    public void AddLines(params LuaElement[] lines) => this.lines.AddRange(lines);

    public override string ToLua(int indentLevel = 0)
    {
        // function(<arguments>) <line1> <line2> ... <lineN> end
        var flattenedlines = lines.Select(line =>
        {
            string lineStr;
            if (line is LuaBlock block)
            {
                var origFlatten = block.Flatten;
                block.Flatten = true;
                lineStr = line.ToLua().TrimEnd('\n');
                block.Flatten = origFlatten;
            }
            else
                lineStr = line.ToLua().TrimEnd('\n');

            return lineStr;
        });

        return $"function({arguments}) {string.Join(" ", flattenedlines)} end";
    }
}

public class LuaFunction : LuaBlock
{
    private readonly LuaIdentifier functionName;
    private readonly LuaComposer arguments = new(", ");
    public bool IsLocal = true; 

    public LuaFunction(LuaIdentifier functionName, params LuaIdentifier[] args)
    {
        this.functionName = functionName;
        AddArguments(args);
    }

    public void AddArgument(LuaIdentifier arg) => arguments.Add(arg);
    public void AddArguments(IEnumerable<LuaIdentifier> args) => arguments.Add(args);
    public void AddArguments(params LuaIdentifier[] args) => arguments.Add(args);

    /*
    <local> function <functionName>(<arguments>)
        <Body>
    end
    */
    public override string ToLua(int indentLevel = 0)
    {
        Header.Clear();
        if (IsLocal)
            Header.Add(LuaConstants.Local);

        Header.Add(LuaConstants.Function, functionName, LuaConstants.OpenParen, arguments, LuaConstants.CloseParen);
        return base.ToLua(indentLevel);
    }
}
