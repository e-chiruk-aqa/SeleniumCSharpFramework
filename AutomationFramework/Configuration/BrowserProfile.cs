using System;
using AutomationFramework.Browsers;
using AutomationFramework.Configuration.WebDriverSettings;
using AutomationFramework.Utilities;

namespace AutomationFramework.Configuration
{
    public class BrowserProfile : IBrowserProfile
    {
        private readonly ISettingsFile settingsFile;

        public BrowserProfile(ISettingsFile settingsFile)
        {
            this.settingsFile = settingsFile;
        }

        public BrowserName BrowserName => (BrowserName)Enum.Parse(typeof(BrowserName), settingsFile.GetValue<string>(".browserName"), ignoreCase: true);

        public IDriverSettings DriverSettings
        {
            get
            {
                switch (BrowserName)
                {
                    case BrowserName.Chrome:
                        return new ChromeSettings(settingsFile);
                    case BrowserName.Firefox:
                        return new FirefoxSettings(settingsFile);
                    default:
                        throw new InvalidOperationException($"There is no assigned behaviour for retrieving DriverSettings for browser {BrowserName}");
                }
            }
        }
    }
}
