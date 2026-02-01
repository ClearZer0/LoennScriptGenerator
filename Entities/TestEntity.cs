using Celeste.Mod.Entities;
using Celeste.Mod.LoennScriptGenerator.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.Entities;

[CustomEntity("LoennScriptGenerator/Test")]
public class TestEntity : Entity
{
    public enum TestEnum
    {
        [Description("First Option = 1")]
        FirstOption,
        SecondOption,
        ThirdOption
    }

    public enum TestEnum2
    {
        FirstOption2,
        SecondOption2,
        ThirdOption2
    }

    [FieldOrder(0)]
    [MinimumValue(0)]
    [PlacementsElement]
    public static int Dash = 1;

    [FieldOrder(1)]
    [Description("iyoiasydotouqw 8608508")]
    [MaximumValue(8126.81276186f)]
    [PlacementsElement]
    public static float Dash2 = 2.9f;

    [PlacementsElement]
    public static bool IsEnabled = true;

    [FieldOrder(5)]
    [Description("81256085hklhiasdgtotasoit")]
    [Editable]
    [PlacementsElement]
    public static TestEnum MyEnum0 = TestEnum.FirstOption;

    [FieldOrder(4)]
    [PlacementsElement]
    public static TestEnum MyEnum1 = TestEnum.SecondOption;

    [FieldOrder(3)]
    [PlacementsElement]
    public static TestEnum2 MyEnum2 = TestEnum2.ThirdOption2;

    [AllowEmpty]
    [UseAlpha]
    [PlacementsElement]
    public static Color Color = Color.Green * 0.5f;

    [MaximumValue(81)]
    [MinimumValue(0)]
    [PlacementsElement]
    [ListElementSeparator(";", depth: 0)]
    [ListMinimumElements(1, depth: 0)]
    [ListMaximumElements(5, depth: 0)]
    [ListMinimumElements(3, depth: 1)]
    [ListMaximumElements(9, depth: 1)]
    public static List<List<int>> ListTest = new()
    {
        new(){ 1,2,3 },
        new(){ 4,5,6,7} 
    };

    public TestEntity(EntityData data, Vector2 offset)
        : base(data.Position + offset)
    {
        Collider = new Hitbox(16f, 16f, 0f, 0f);

        Logger.Info("", data.Attr("myEnum0"));
        Logger.Info("", data.Enum<TestEnum>("myEnum0").ToString());
        Logger.Info("", data.Attr("myEnum1"));
        Logger.Info("", data.Enum<TestEnum>("myEnum1").ToString());
    }

    public override void Render()
    {
        base.Render();
        var c = Color.LightBlue;
        Draw.Rect(Collider, c * 0.5f);
        Draw.HollowRect(Collider, c);
    }
}
