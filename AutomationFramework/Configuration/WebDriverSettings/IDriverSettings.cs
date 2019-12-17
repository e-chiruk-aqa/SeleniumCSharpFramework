using OpenQA.Selenium;
using WebDriverManager.Helpers;

namespace AutomationFramework.Configuration.WebDriverSettings
{
    public interface IDriverSettings
    {
        string WebDriverVersion { get; }

        Architecture SystemArchitecture { get; }

        DriverOptions DriverOptions { get; }

        string DownloadDir { get; }

        string DownloadDirCapabilityKey { get; }
    }
}
