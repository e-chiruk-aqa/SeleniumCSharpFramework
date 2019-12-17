using System.Reflection;
using AutomationFramework.Utilities;

namespace AutomationFramework.Configuration
{
    public static class Settings
    {
        public static ISettingsFile GetSettings()
        {
            var profileNameFromEnvironment = EnvironmentConfiguration.GetVariable("profile");
            var settingsProfile = profileNameFromEnvironment == null ? "settings.json" : $"settings.{profileNameFromEnvironment}.json";
            var jsonFile = FileReader.IsResourceFileExist(settingsProfile)
                ? new JsonSettingsFile(settingsProfile)
                : new JsonSettingsFile($"Resources.{settingsProfile}", Assembly.GetCallingAssembly());
            return jsonFile;
        }
    }
}
