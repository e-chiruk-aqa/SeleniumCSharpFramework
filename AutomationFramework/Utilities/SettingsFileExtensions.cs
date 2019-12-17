using System.Collections.Generic;

namespace AutomationFramework.Utilities
{
    public static class SettingsFileExtensions
    {
        public static T GetValueOrNew<T>(this ISettingsFile file, string path) where T : new()
        {
            return GetValueOrDefault(file, path, new T());
        }

        public static IReadOnlyList<T> GetValueListOrEmpty<T>(this ISettingsFile file, string path)
        {
            return file.IsValuePresent(path) ? file.GetValueList<T>(path) : new List<T>();
        }

        public static T GetValueOrDefault<T>(this ISettingsFile file, string path, T defaultValue = default(T))
        {
            return file.IsValuePresent(path) ? file.GetValue<T>(path) : defaultValue;
        }
    }
}
