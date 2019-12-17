using AutomationFramework.Browsers;
using AutomationFramework.Configuration.WebDriverSettings;

namespace AutomationFramework.Configuration
{
    public interface IBrowserProfile
    {
        BrowserName BrowserName { get; }

        IDriverSettings DriverSettings { get; }
    }
}
