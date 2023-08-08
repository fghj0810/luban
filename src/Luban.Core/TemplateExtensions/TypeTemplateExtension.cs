using Luban.CodeFormat;
using Luban.Defs;
using Luban.Types;
using Scriban.Runtime;

namespace Luban.TemplateExtensions;

public class TypeTemplateExtension : ScriptObject
{
    public static bool NeedMarshalBoolPrefix(TType type)
    {
        return type.IsNullable;
    }
    
    public static string FormatFieldName(ICodeStyle codeStyle, string name)
    {
        return codeStyle.FormatField(name);
    }

    public static string FormatPropertyName(ICodeStyle codeStyle, string name)
    {
        return codeStyle.FormatProperty(name);
    }

    public static string FormatEnumItemName(ICodeStyle codeStyle, string name)
    {
        return codeStyle.FormatEnumItemName(name);
    }
    
    public static bool CanGenerateRef(DefField field)
    {
        if (field.CType.IsCollection)
        {
            return false;
        }

        return GetRefTable(field) != null;
    }
    
    public static DefTable GetRefTable(DefField field)
    {
        if (field.CType.GetTag("ref") is { } value && GenerationContext.Current.Assembly.GetCfgTable(value) is { } cfgTable)
        {
            return cfgTable;
        }
        return null;
    }
    
    public static TType GetRefType(DefField field)
    {
        return GetRefTable(field)?.ValueTType;
    }
    
    public static bool IsFieldBeanNeedResolveRef(DefField field)
    {
        return field.CType is TBean bean && bean.DefBean.TypeMappers == null && !bean.DefBean.IsValueType;
    }
    
    public static bool IsFieldArrayLikeNeedResolveRef(DefField field)
    {
        return field.CType.ElementType is TBean bean && bean.DefBean.TypeMappers == null && !bean.DefBean.IsValueType && field.CType is not TMap;
    }

    public static bool IsFieldMapNeedResolveRef(DefField field)
    {
        return field.CType is TMap { ValueType: TBean bean } && bean.DefBean.TypeMappers == null && !bean.DefBean.IsValueType;
    }
}