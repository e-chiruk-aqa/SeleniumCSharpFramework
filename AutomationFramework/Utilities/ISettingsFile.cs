using System.Collections.Generic;

namespace AutomationFramework.Utilities
{
    public interface ISettingsFile
    {
        T GetValue<T>(string path);

        IReadOnlyList<T> GetValueList<T>(string path);

        IReadOnlyDictionary<string, T> GetValueDictionary<T>(string path);

        bool IsValuePresent(string path);
    }
}
