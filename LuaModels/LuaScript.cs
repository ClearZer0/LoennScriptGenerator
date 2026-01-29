using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LuaModels;

public class LuaScript
{
    protected readonly List<LuaElement> elements = new();

    public void AddNewLine() => elements.Add(LuaNewLine.Instance);
    public void Add(LuaElement element) => elements.Add(element);
    public void Add(IEnumerable<LuaElement> elements) => this.elements.AddRange(elements);
    public void Add(params LuaElement[] elements) => this.elements.AddRange(elements);

    public virtual string ToLua()
    {
        StringBuilder sb = new StringBuilder();
        foreach (LuaElement element in elements)
            sb.Append(element.ToLua());
        return sb.ToString();
    }
    public override string ToString() => ToLua();
}

public class LuaModule : LuaScript
{
    protected readonly string ModuleName;
    protected readonly List<LuaAssignment> requires = new();

    public LuaModule(string moduleName)
    {
        ModuleName = moduleName;

        // local <moduleName> = {}
        Add(new LuaAssignment(ModuleName, new LuaArray()));
    }

    public void AddRequire(string varName, string modulePath) => requires.Add(new LuaAssignment(varName, new LuaFunctionCall("require", new LuaString(modulePath))));

    public override string ToLua()
    {
        StringBuilder sb = new StringBuilder();

        // requires
        foreach (LuaAssignment require in requires)
            sb.Append(require.ToLua());
        if (requires.Count > 0)
            sb.AppendLine();

        // module body
        foreach (LuaElement element in elements)
            sb.Append(element.ToLua());
        sb.AppendLine();

        // return module
        sb.Append($"return {ModuleName}");

        return sb.ToString();
    }
}