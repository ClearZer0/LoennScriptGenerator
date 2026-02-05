using Celeste.Mod.LoennScriptGenerator.LuaModels;
using Celeste.Mod.LoennScriptGenerator.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
/*        var result = FakeExecute(metadata.Config.SpriteFunction);
        var spriteMetadata = result.metadata;*/
        var rawExpression = metadata.Config.__SpriteFuncExpression;
        if (rawExpression == string.Empty)
            return null;

        // function <module.Name>.sprite(room, entity)
        var spriteFunc = new LuaFunction($"{metadata.Name}.sprite", "room", "entity") { IsLocal = false };

        // if container
        var ifStack = new Stack<LuaIf>();
        var rawList = rawExpression.Split('\n').ToList();
        var currentLineIndex = rawList.Select(l => l.Trim()).ToList().IndexOf("{") + 1;
        ConvertBlock(spriteFunc);
        return spriteFunc;

        // recusive convert controll flow statement
        void ConvertBlock(LuaBlock block)
        {
            for (; currentLineIndex < rawList.Count; currentLineIndex++)
            {
                string rawLine = rawList[currentLineIndex];
                var line = rawLine.Trim();
                 
                // end of block
                if (line == "}")
                {
                    if (block is not LuaIf)
                        return;

                    var nextLine = string.Empty;
                    var tempIndex = currentLineIndex + 1;
                    while (nextLine == string.Empty && tempIndex < rawList.Count)
                    {
                        nextLine = rawList[tempIndex].Trim();
                        tempIndex++;
                    }

                    if (!nextLine.StartsWith("else if") && !nextLine.StartsWith("else"))
                        ifStack.Pop();
                    return;
                }

                if (line == string.Empty || line == "{" || line.EndsWith('{'))
                    continue;

                // check line is controll flow statement
                var macth = Regex.Match(line, @"^\s*(if|else\s+if|else)\b");
                if (macth.Success)
                {
                    currentLineIndex++;
                    var lParen = line.IndexOf("(");
                    var rParen = line.LastIndexOf(")");
                    var keyword = macth.Groups[1].Value;
                    // else dont have ()
                    var condition = lParen == -1 ? string.Empty : line.Substring(lParen + 1, rParen - lParen - 1);

                    var statementBlock = ConvertStatementHeader(keyword, condition);
                    ConvertBlock(statementBlock);
                    if (statementBlock is not LuaIf.FragmentBlock)
                        block.AddLine(statementBlock);
                }
                else
                {
                    // ...
                    block.AddLine(new LuaIdentifier(line));

                }
            }
        }

        LuaBlock ConvertStatementHeader(string keyword, string condition)
        {
            var statementBlock = new LuaBlock();
            switch (keyword)
            {
            // convert condition here
            case "if":
                statementBlock = new LuaIf(condition);
                ifStack.Push((LuaIf)statementBlock);
                break;
            case "else if":
                statementBlock = ifStack.Peek().AddElseIf(condition);
                break;
            case "else":
                statementBlock = ifStack.Peek().AddElse();
                break;
            }

            return statementBlock;
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
