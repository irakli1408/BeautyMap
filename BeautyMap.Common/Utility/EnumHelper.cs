using System.ComponentModel;
using System.Reflection;

namespace BeautyMap.Common.Utility
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetEnumDefaultValue<T>(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DefaultValueAttribute attribute = field.GetCustomAttribute<DefaultValueAttribute>();
            return attribute == null ? default(T) : (T)attribute.Value;
        }
    }
}
