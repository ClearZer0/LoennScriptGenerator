using Celeste.Mod.Entities;
using Celeste.Mod.LoennScriptGenerator.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Celeste.Mod.LoennScriptGenerator.Metadatas.ElementMetadata;

namespace Celeste.Mod.LoennScriptGenerator.Metadatas;

public class ElementMetadata
{
    public enum ElementType
    {
        Int,
        Float,
        String,
        Bool,
        Enum,
        Color,
        List
    }

    public string Name;
    public string Description = string.Empty;
    public int? FieldOrder;

    public ElementType Type;
    public object? DefaultValue;

    public float? MinimumValue;
    public float? MaximumValue;

    public string? EnumOptionsName;
    public List<string>? EnumOptionDisplayNames;
    public List<string>? EnumOptions;
    public bool Editable;

    public bool UseAlpha;
    public bool AllowEmpty;

    public string? ListElementSeparator;
    public int? ListMinimumElements;
    public int? ListMaximumElements;
    public ElementMetadata? ListElementMetadata;

    public ElementMetadata(string name, ElementType type)
    {
        Name = name;
        Type = type;
    }
}

public class EntityScriptMetadata
{
    // reflection from CustomEntityAttribute.ID[0]
    public string CustomEntityName = string.Empty;

    public string ClassName;
    public string Name;
    public List<ElementMetadata> Elements = new();
    public LoennEntityConfig Config;

    private static readonly BindingFlags flags = BindingFlags.Public | BindingFlags.Static;

    private EntityScriptMetadata(string customEntityName, string className, LoennEntityConfig config)
    {
        CustomEntityName = customEntityName;
        ClassName = className;
        Name = LoennScriptGeneratorUtils.LowerCamelCase(className);
        Config = config;
    }

    public static EntityScriptMetadata Create<TEntity>() where TEntity : Entity
    {
        var type = typeof(TEntity);
        var customEntityAttr = type.GetCustomAttribute<CustomEntityAttribute>();
        var fields = type.GetFields(flags);

        // config reflect from static method
        var configMethodInfo = type.GetMethods(flags).Where(m => m.ReturnType == typeof(LoennEntityConfig)).FirstOrDefault();
        var config = configMethodInfo != null ? (LoennEntityConfig)configMethodInfo.Invoke(null, null)! : new LoennEntityConfig();
        var metadata = new EntityScriptMetadata(customEntityAttr!.IDs[0], type.Name, config);

        foreach (var field in fields.Where(f => f.IsDefined(typeof(PlacementsElementAttribute)))) 
        {
            var attributes = field.GetCustomAttributes();
            var elementMetadata = CreateElementMetadata(field, attributes);
            metadata.Elements.Add(elementMetadata);
        }

        return metadata;
    }

    private static ElementMetadata CreateElementMetadata(FieldInfo field, IEnumerable<Attribute> attributes)
    {
        var elementType = GetElementType(field.FieldType);
        var rootMetadata = new ElementMetadata(LoennScriptGeneratorUtils.LowerCamelCase(field.Name.TrimStart('_')), elementType);
        rootMetadata.DefaultValue = field.GetValue(null);

        SetFieldInfomation(rootMetadata, field.FieldType);
        return rootMetadata;

        void SetFieldInfomation(ElementMetadata metadata, Type currentType, int depth = 0)
        {
            // list
            if (metadata.Type == ElementType.List)
            {
                foreach (var attribute in attributes)
                {
                    // check nested depth
                    if (attribute is not ListElementAttrBase listAttr || listAttr.Depth != depth)
                        continue;

                    object _ = listAttr switch
                    {
                        ListElementSeparatorAttribute listSepAttr => metadata.ListElementSeparator = listSepAttr.Separator,
                        ListMinimumElementsAttribute listMinAttr => metadata.ListMinimumElements = listMinAttr.MinimumElements,
                        ListMaximumElementsAttribute listMaxAttr => metadata.ListMaximumElements = listMaxAttr.MaximumElements,
                        _ => 0
                    };
                }

                // recursive set list element's metadata
                var innerType = currentType.GetGenericArguments()[0];
                metadata.ListElementMetadata = new ElementMetadata(metadata.Name, GetElementType(innerType));
                SetFieldInfomation(metadata.ListElementMetadata, innerType, depth + 1);
            }
            else
            {
                if (metadata.Type == ElementType.Enum)
                {
                    rootMetadata.EnumOptionsName = $"{LoennScriptGeneratorUtils.LowerCamelCase(currentType.Name)}Options";
                    rootMetadata.EnumOptions = Enum.GetNames(currentType).ToList();
                    rootMetadata.EnumOptionDisplayNames = new();

                    var enumFields = field.FieldType.GetFields(flags);
                    foreach (var field in enumFields)
                    {
                        var descAttr = field.GetCustomAttribute<DescriptionAttribute>();
                        var displayName = descAttr != null ? $"[\"{descAttr.Description}\"]" : field.Name;
                        rootMetadata.EnumOptionDisplayNames!.Add(displayName);
                    }
                }

                foreach (var attribute in attributes)
                {
                    object _ = attribute switch
                    {
                        FieldOrderAttribute order => metadata.FieldOrder = order.Order,
                        DescriptionAttribute d => metadata.Description = d.Description,
                        MinimumValueAttribute min => metadata.MinimumValue = min.Value,
                        MaximumValueAttribute max => metadata.MaximumValue = max.Value,
                        EditableAttribute => metadata.Editable = true,
                        UseAlphaAttribute => metadata.UseAlpha = true,
                        AllowEmptyAttribute => metadata.AllowEmpty = true,
                        _ => 0
                    };
                }
            }
        }
    }

    private static ElementType GetElementType(Type type) => type switch
    {
        Type t when t == typeof(int) => ElementType.Int,
        Type t when t == typeof(float) => ElementType.Float,
        Type t when t == typeof(string) => ElementType.String,
        Type t when t == typeof(bool) => ElementType.Bool,
        Type t when t.IsEnum => ElementType.Enum,
        Type t when t == typeof(Color) => ElementType.Color,
        Type t when t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>) => ElementType.List,
        _ => throw new NotSupportedException($"Field type '{type.FullName}' is not supported for element metadata.")
    };
}