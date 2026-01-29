using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LuaModels;

public class LuaBlock : LuaElement
{
    /*
    do
        ...
    end
    */
    protected readonly LuaComposer Header = new(" ", LuaConstants.Do);
    protected string Footer = "end";
    protected readonly List<LuaElement> Body = new();

    public bool Flatten = false;

    public void AddLine(LuaElement line) => Body.Add(line);
    public void AddLines(IEnumerable<LuaElement> lines) => Body.AddRange(lines);
    public void AddLines(params LuaElement[] lines) => Body.AddRange(lines);


    public override string ToLua(int indentLevel = 0)
    {
        // <header> <bodyLine1> <bodyLine2> ... <bodyLineN> <footer>
        if (Flatten)
        {
            if (Body.Count == 0)
                return $"{Header} {Footer}";

            var flattenedlines = Body.Select(line => line.ToLua().TrimEnd('\n'));
            return $"{Header} {string.Join(" ", flattenedlines)} {Footer}\n";
        }

        /*
        <header>
            <bodyLine1>
            <bodyLine2>
            ...
            <bodyLineN>
        <footer>
        */
        string outerIndent = GetIndent(indentLevel);
        string innerIndent = GetIndent(indentLevel + 1);

        if (Body.Count == 0)
            return $"{outerIndent}{Header}\n{outerIndent}{Footer}\n";

        var lines = Body.Select(line =>
        {
            var lineStr = line.ToLua(indentLevel + 1);

            // trim trailing newlines for assignments and nested blocks to avoid extra spacing
            if (line is LuaAssignment or LuaBlock)
                lineStr = lineStr.TrimEnd('\n');
            return $"{innerIndent}{lineStr}";
        });

        var newline = Footer == string.Empty ? "" : "\n";
        return $"{outerIndent}{Header}\n{string.Join("\n", lines)}\n{outerIndent}{Footer}{newline}";
    }
}

public class LuaIf : LuaBlock
{
    public class FragmentBlock : LuaBlock
    {
        public FragmentBlock(params LuaIdentifier[] header)
        {
            Header.Clear();
            Header.Add(header);
            Footer = string.Empty;
        }
    }

    private readonly List<FragmentBlock> ElseIfBlocks = new();
    private FragmentBlock? ElseBlock = null;

    /*
    if <condition> then
        ...
    elseif <condition> then
        ...
    else
        ...
    end
    */
    public LuaIf(LuaIdentifier condition)
    {
        Header.Clear();
        Header.Add(LuaConstants.If, condition, LuaConstants.Then);
        Footer = string.Empty;
    }

    public FragmentBlock AddElseIf(LuaIdentifier condition)
    {
        var elseifBlock = new FragmentBlock(LuaConstants.ElseIf, condition, LuaConstants.Then);
        ElseIfBlocks.Add(elseifBlock);
        return elseifBlock;
    }

    public FragmentBlock AddElse()
    {
        var elseBlock = new FragmentBlock(LuaConstants.Else);
        ElseBlock = elseBlock;
        return elseBlock;
    }

    public override string ToLua(int indentLevel = 0)
    {
        StringBuilder sb = new StringBuilder();

        // if
        sb.Append(base.ToLua(indentLevel));
        // elseifs
        foreach (var elseif in ElseIfBlocks)
            sb.Append(elseif.ToLua(indentLevel));
        // else
        if (ElseBlock != null)
            sb.Append(ElseBlock.ToLua(indentLevel));

        // end
        string outerIndent = GetIndent(indentLevel);
        sb.Append($"{outerIndent}end\n");

        return sb.ToString();
    }
}

public class LuaFor : LuaBlock
{
    /*
    for <var> = <start>, <end>, <step> do
        ...
    end
    */
    public LuaFor(LuaIdentifier var, LuaValue start, LuaValue end, LuaValue? step = null)
    {
        if (start is LuaString s1) start = s1.ToIdentifier();
        if (end is LuaString s2) end = s2.ToIdentifier();
        if (step is LuaString s3) step = s3.ToIdentifier();

        // <start>, <end>, <step>
        var nums = new LuaComposer(", ");
        nums.Add(start, end);
        if (step != null)
            nums.Add(step);

        // for <var> = <nums> do
        Header.Clear();
        Header.Add(LuaConstants.For, var, LuaConstants.Equal, nums, LuaConstants.Do);
    }

    /*
    for <vars> in <iterator> do
        ...
    end
    */
    public LuaFor(LuaIdentifier vars, LuaFunctionCall iterator)
    {
        Header.Clear();
        Header.Add(LuaConstants.For, vars, LuaConstants.In, iterator, LuaConstants.Do);
    }
}

public class LuaWhile : LuaBlock
{
    /* 
    while <condition> do
        ...
    end
    */
    public LuaWhile(LuaIdentifier condition)
    {
        Header.Clear();
        Header.Add(LuaConstants.While, condition, LuaConstants.Do);
    }
}

public class LuaRepeat : LuaBlock
{
    /*
    repeat
        ...
    until <condition>
    */
    public LuaRepeat(LuaIdentifier condition)
    {
        Header.Clear();
        Header.Add(LuaConstants.Repeat);
        Footer = $"until {condition.ToLua()}";
    }
}
