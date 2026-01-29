using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.API;

#region Placements
// use for decorating placement elements
// supprots int, float. string, bool, enum, Color, List<T> (where T is one of the supported types)
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class PlacementsElementAttribute : Attribute
{
}

// set element's tooltip description
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class DescriptionAttribute : Attribute
{
    public readonly string Description;
    public DescriptionAttribute(string description) => Description = description;
}
#endregion

#region FieldInfomation
// use for set minimum value for int or float types
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class MinimumValueAttribute : Attribute
{
    public readonly float Value;
    public MinimumValueAttribute(float value) => Value = value;
}

// use for set maximum value for int or float types
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class MaximumValueAttribute : Attribute
{
    public readonly float Value;
    public MaximumValueAttribute(float value) => Value = value;
}

// use for enum type (options in Loenn)
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class EditableAttribute : Attribute
{
}

// use for set default value for List<T> types
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ListDefaultValueAttribute : ListElementAttrBase
{
    public readonly int DefaultValueIndex;
    public ListDefaultValueAttribute(int defaultValueIndex, int depth = 0) : base(depth) => DefaultValueIndex = defaultValueIndex;
}

// use for set minimum elements for List<T> types
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ListMinimumElementsAttribute : ListElementAttrBase
{
    public readonly int MinimumElements;
    public ListMinimumElementsAttribute(int minimumElements, int depth = 0) : base(depth) => MinimumElements = minimumElements;
}

// use for set maximum elements for List<T> types
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ListMaximumElementsAttribute : ListElementAttrBase
{
    public readonly int MaximumElements;
    public ListMaximumElementsAttribute(int maximumElements, int depth = 0) : base(depth) => MaximumElements = maximumElements;
}

public abstract class ListElementAttrBase : Attribute
{
    public readonly int Depth;
    public ListElementAttrBase(int depth) => Depth = depth;
}
#endregion