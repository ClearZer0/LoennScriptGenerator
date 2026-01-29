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
    public string Description { get; }
    public DescriptionAttribute(string description) => Description = description;
}
#endregion

#region FieldInfomation
// use for set minimum value for int or float types
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class MinimumValueAttribute : Attribute
{
    public float Value { get; }
    public MinimumValueAttribute(float value) => Value = value;
}

// use for set maximum value for int or float types
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class MaximumValueAttribute : Attribute
{
    public float Value { get; }
    public MaximumValueAttribute(float value) => Value = value;
}

// use for enum type (options in Loenn)
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class EditableAttribute : Attribute
{
}

// use for set default value for List<T> types
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ListDefaultValueAttribute : Attribute
{
    public int DefaultValueIndex { get; }
    public ListDefaultValueAttribute(int defaultValueIndex) => DefaultValueIndex = defaultValueIndex;
}

// use for set minimum elements for List<T> types
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ListMinimumElementsAttribute : Attribute
{
    public int MinimumElements { get; }
    public ListMinimumElementsAttribute(int minimumElements) => MinimumElements = minimumElements;
}

// use for set maximum elements for List<T> types
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ListMaximumElementsAttribute : Attribute
{
    public int MaximumElements { get; }
    public ListMaximumElementsAttribute(int maximumElements) => MaximumElements = maximumElements;
}
#endregion