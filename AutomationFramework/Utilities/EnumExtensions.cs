using System;
using NUnit.Framework;

namespace AutomationFramework.Utilities
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this object value) where T : struct, IConvertible
        {
            return value is int
                ? (T)(object)(int)(long)value
                : value.ToString().ToEnum<T>();
        }

        public static T ToEnum<T>(this string value) where T : struct, IConvertible
        {
            var type = typeof(T);
            Assert.IsTrue(type.IsEnum, "T must be an enum type");
            return (T)Enum.Parse(type, value, ignoreCase: true);
        }
    }
}
