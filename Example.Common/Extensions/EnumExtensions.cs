using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Example.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T value) where T : Enum
        {
            return value
                       .GetType()
                       .GetMember(value.ToString())
                       .FirstOrDefault()
                       ?.GetCustomAttribute<DescriptionAttribute>()
                       ?.Description
                   ?? value.ToString();
        }
    }
}
