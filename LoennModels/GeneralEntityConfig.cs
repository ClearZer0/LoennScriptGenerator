using Celeste.Mod.LoennScriptGenerator.LuaModels;
using Celeste.Mod.LoennScriptGenerator.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LoennModels;

public static class GeneralEntityConfig
{
    public static LuaFragment Creste(EntityScriptMetadata metadata)
    {
        var fragment = new LuaFragment();
        var lines = new List<LuaAssignment>();
        var config = metadata.Config;
        var prefix = metadata.Name;

        // basic
        if (config.Depth is { } depth)
            lines.Add(AddAssignment("depth", depth));
        if (config.CanResize is { } cr)
            lines.Add(AddAssignment("canResize", new LuaArray(cr.horizontal, cr.vertical)));
        AddToFragment();

        // simple rendering
        if (config.Texture is { } texture)
            lines.Add(AddAssignment("texture", texture));
        if (config.Justification is { } jjustification)
            lines.Add(AddAssignment("justification", new LuaArray(jjustification.x, jjustification.y)));
        if (config.Scale is { } scale)
            lines.Add(AddAssignment("scale", new LuaArray(scale.x, scale.y)));
        if (config.Offset is { } offset)
            lines.Add(AddAssignment("offset", new LuaArray(offset.x,  offset.y)));
        if (config.Rotation is { } rotation)
            lines.Add(AddAssignment("rotation", rotation));
        if (config._Color is { } color)
            lines.Add(AddAssignment("color", ConvertColor(color)));
        AddToFragment();

        // rect rendering
        if (config.FillColor is { } fc)
            lines.Add(AddAssignment("fillColor", ConvertColor(fc)));
        if (config.BorderColor is { } bc)
            lines.Add(AddAssignment("borderColor", ConvertColor(bc)));
        AddToFragment();

        // node
        if (config.NodeLimits is { } nl)
            lines.Add(AddAssignment("nodeLimits", new LuaArray(nl.min, nl.max)));
        if (config.NodeVisibility is { } nv)
            lines.Add(AddAssignment("nodeVisibility", LoennScriptGeneratorUtils.LowerCamelCase(nv.ToString())));
        if (config.NodeLineRenderType is { } nlrt)
            lines.Add(AddAssignment("nodeLineRenderType", LoennScriptGeneratorUtils.LowerCamelCase(nlrt.ToString())));
        AddToFragment();

        return fragment;

        LuaArray ConvertColor(Color c) => new LuaArray(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        LuaAssignment AddAssignment(LuaIdentifier varName, LuaValue value) => new LuaAssignment(LuaComposer.SimplePrefix(prefix, varName), value) { IsLocal = false};
        void AddToFragment()
        {
            if (lines.Count > 0)
            {
                fragment.Add(lines);
                fragment.AddNewLine();
                lines.Clear();
            }
        }
    }
}
