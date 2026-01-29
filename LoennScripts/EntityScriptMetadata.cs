using Celeste.Mod.Entities;
using Celeste.Mod.LoennScriptGenerator.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LoennScripts;

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

    public ElementType Type;
    public object? DefaultValue;

    public float? MinimumValue;
    public float? MaximumValue;

    public bool Editable;
    public List<string>? EnumOptions;

    public List<object>? ListElements;
    public int? ListDefaultValueIndex;
    public int? ListMinimumElements;
    public int? ListMaximumElements;
    public ElementMetadata? ListElementsMetadata;

    public ElementMetadata(string name, ElementType type)
    {
        Name = name;
        Type = type;
    }
}

public class EntityScriptMetadata
{
    public string Name;
    public List<ElementMetadata> Elements = new();
    public LoennEntityConfig Config;

    private static readonly BindingFlags flags = BindingFlags.Public | BindingFlags.Static;

    public EntityScriptMetadata(string name, LoennEntityConfig config)
    {
        Name = name;
        Config = config;
    }

    public static EntityScriptMetadata Create<TEntity>() where TEntity : Entity
    {
        var type = typeof(TEntity);
        var customEntityAttr = type.GetCustomAttribute<CustomEntityAttribute>();
        var config = CreateConfig(type, customEntityAttr!.IDs[0]);
        var metadata = new EntityScriptMetadata(ToLowerCamelCase(type.Name), config);

        var fields = type.GetFields(flags).Where(f => f.IsDefined(typeof(PlacementsElementAttribute)));
        foreach ( var field in fields ) 
        {
            var attributes = field.GetCustomAttributes().ToArray();
            var elementMetadata = CreateElementMetadata(field, attributes);
            metadata.Elements.Add(elementMetadata);
        }

        return metadata;
    }

    private static ElementMetadata CreateElementMetadata(FieldInfo field, Attribute[] attributes)
    {
        return null!;
    }

    private static LoennEntityConfig CreateConfig(Type entityType, string customEntityName)
    {
        var configField = entityType.GetFields(flags).FirstOrDefault(f => f.FieldType == typeof(LoennEntityConfig));
        if (configField == null)
            return new LoennEntityConfig { CustomEntityName = customEntityName };

        var config = (LoennEntityConfig)configField.GetValue(null)!;
        config.CustomEntityName = customEntityName;
        return config;
    }

    private static string ToLowerCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
            return input;
        return char.ToLower(input[0]) + input.Substring(1);
    }
}