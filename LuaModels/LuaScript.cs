using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LuaModels;

public class LuaScript
{
    protected readonly LuaFragment elements = new();    

    public void AddNewLine() => elements.AddNewLine();
    public void Add(LuaElement element) => elements.Add(element);
    public void Add(IEnumerable<LuaElement> elements) => this.elements.Add(elements);
    public void Add(params LuaElement[] elements) => this.elements.Add(elements);

    public virtual string ToLua() => elements.ToLua();
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
        AddNewLine();
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
        sb.Append(elements.ToLua());
        sb.AppendLine();

        // return module
        sb.Append($"return {ModuleName}");

        return sb.ToString();
    }
}