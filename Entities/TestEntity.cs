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

    [MinimumValue(0)]
    [PlacementsElement]
    public static int Dash = 1;

    [MaximumValue(8126.81276186f)]
    [PlacementsElement]
    public static float Dash2 = 2.9f;

    [PlacementsElement]
    public static bool IsEnabled = true;

    [Editable]
    [PlacementsElement]
    public static TestEnum MyEnum0 = TestEnum.FirstOption;

    [PlacementsElement]
    public static TestEnum MyEnum1 = TestEnum.SecondOption;

    [PlacementsElement]
    public static TestEnum2 MyEnum2 = TestEnum2.ThirdOption2;

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
}
