using Celeste.Mod.LoennScriptGenerator.LoennModels;
using Celeste.Mod.LoennScriptGenerator.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.API;

public class LoennEntityConfig
{
    public enum NodeVisibilityEnum
    {
        Never,
        Selected,
        Always
    }

    public enum NodeLineRenderTypeEnum
    {
        Line,
        Fan,
        Circle
    }

    public int? Depth;
    public (bool horizontal, bool vertical) CanResize;

    public string? Texture;
    public (float x, float y)? Justification;
    public (int x, int y)? Scale;
    public (int x, int y)? Offset;
    public float? Rotation;

    public Color? _Color;
    public Color? FillColor;
    public Color? BorderColor;

    public (int min, int max)? NodeLimits;
    public NodeVisibilityEnum? NodeVisibility;
    public NodeLineRenderTypeEnum? NodeLineRenderType;

    // (room, entity, spriteFunc)
    public string __SpriteFuncExpression = string.Empty;
    public void SetSpriteFunction(Action<FakeLuaTable, FakeLuaTable, SpriteFunctionMetadata> spriteFunc, [CallerArgumentExpression(nameof(spriteFunc))] string _expression = "")
        => __SpriteFuncExpression = _expression;
}
