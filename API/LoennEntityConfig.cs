using Celeste.Mod.LoennScriptGenerator.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public string? Texture;
    public (float x, float y)? Justification;
    public (int x, int y)? Scale;
    public (int x, int y)? Offset;
    public float? Rotation;

    public Color? Color;
    public Color? FillColor;
    public Color? BorderColor;

    public (int min, int max)? NodeLimit;
    public NodeVisibilityEnum? NodeVisibility;
    public NodeLineRenderTypeEnum? NodeLineRenderType;

    private SpriteFunctionMetadata? spriteFunction;
    public SpriteFunctionMetadata SpriteFunction => spriteFunction ?? (spriteFunction = new());
}
