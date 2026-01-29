using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LuaModels;

public abstract class LuaValue : LuaElement
{
    public static implicit operator LuaValue(string s) => new LuaString(s);
    public static implicit operator LuaValue(int i) => new LuaInt(i);
    public static implicit operator LuaValue(float f) => new LuaFloat(f);
    public static implicit operator LuaValue(bool b) => new LuaBoolean(b);
}

public class LuaIdentifier : LuaValue
{
    private readonly string value;

    public LuaIdentifier(string value) => this.value = value;
    public override string ToLua(int indentLevel = 0) => value.ToString();
    public static implicit operator LuaIdentifier(string s) => new LuaIdentifier(s);
    public static implicit operator LuaIdentifier(LuaComposer composer) => new LuaIdentifier(composer.ToLua());
}

public class LuaInt : LuaValue
{
    private readonly int value;

    public LuaInt(int value) => this.value = value;
    public override string ToLua(int indentLevel = 0) => value.ToString(CultureInfo.InvariantCulture);
}

public class LuaFloat : LuaValue
{
    private readonly float value;

    public LuaFloat(float value) => this.value = value;
    public override string ToLua(int indentLevel = 0) => value.ToString(CultureInfo.InvariantCulture);
}

public class LuaString : LuaValue
{
    private readonly string value;

    public LuaString(string value) => this.value = value;
    public LuaIdentifier ToIdentifier() => new LuaIdentifier(value);
    public override string ToLua(int indentLevel = 0) => $"\"{EscapeString(value)}\"";
    private static string EscapeString(string input) => 
        input.Replace("\\", "\\\\")
        .Replace("\"", "\\\"")
        .Replace("'", "\\'")
        .Replace("\n", "\\n")
        .Replace("\r", "\\r");
}

public class LuaBoolean : LuaValue
{
    private readonly bool value;

    public LuaBoolean(bool value) => this.value = value;
    public override string ToLua(int indentLevel = 0) => value.ToString().ToLowerInvariant();
}

public class LuaArray : LuaValue
{
    private readonly List<LuaValue> elements = new();
    public bool IsEmpty => elements.Count == 0;

    public void AddElement(LuaValue value) => elements.Add(value);

    // { value1, value2, ..., valuen }
    public override string ToLua(int indentLevel = 0)
    {
        if (elements.Count == 0) 
            return "{}";

        var items = elements.Select(e => e.ToLua());
        return $"{{ {string.Join(", ", items)} }}";
    }
}

public class LuaTable : LuaValue
{
    private readonly List<(LuaValue key, LuaValue value)> entries = new();
    public bool IsEmpty => entries.Count == 0;

    public LuaValue this[LuaIdentifier identifier] { set => AddEntry(identifier, value); }
    public void AddEntry(LuaValue key, LuaValue value) => entries.Add((key, value));

    /*
    {
        key1 = value1,
        key2 = value2,
        ...
        nestedTable =
        {
            subkey1 = subvalue1,
            subkey2 = subvalue2
        },
        keyn = valuen
    }
    */
    public override string ToLua(int indentLevel = 0)
    {
        if (entries.Count == 0)
            return "{}";

        string outerIndent = GetIndent(indentLevel);
        string innerIndent = GetIndent(indentLevel + 1);

        var entryLines = entries.Select(entry =>
        {
            string keyStr = entry.key.ToLua();
            string valueStr = entry.value.ToLua(indentLevel + 1);
            return $"{innerIndent}{keyStr} = {valueStr}";
        });
        return $"\n{outerIndent}{{\n{string.Join(",\n", entryLines)}\n{outerIndent}}}";
    }
}

public class LuaComposer : LuaValue
{
    public static LuaComposer NoSeparator => new LuaComposer("");

    private readonly string separator;
    private readonly List<LuaValue> values = new();
    public bool IsEmpty => values.Count == 0;

    public static LuaComposer SimplePrefix(params LuaIdentifier[] identifiers) => new LuaComposer(".", identifiers);
    public static LuaComposer SimpleExpression(params LuaIdentifier[] identifiers) => new LuaComposer(" ", identifiers);
    public LuaComposer(string separator = " ", params LuaValue[] values)
    {
        this.separator = separator;
        this.values.AddRange(values);
    }

    public void Clear() => values.Clear();
    public void Add(LuaValue value) => values.Add(value);
    public void Add(IEnumerable<LuaValue> values) => this.values.AddRange(values);
    public void Add(params LuaValue[] values) => this.values.AddRange(values);
    public void AddIdentifiers(params LuaIdentifier[] identifiers) => values.AddRange(identifiers);
    public void AddWithDot(LuaIdentifier identifier) => Add(LuaConstants.Dot, identifier);
    public void AddWithSemicolon(LuaIdentifier identifier) => Add(LuaConstants.Semicolon, identifier);
    public override string ToLua(int indentLevel = 0) => IsEmpty ? string.Empty : string.Join(separator, values.Select(v => v.ToLua(indentLevel)));
}