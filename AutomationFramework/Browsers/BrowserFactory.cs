using System;
using System.IO;
using AutomationFramework.Configuration;
using AutomationFramework.Configuration.WebDriverSettings;
using AutomationFramework.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace AutomationFramework.Browsers
{
    public class BrowserFactory
    {
        public static Logger Logger = Logger.Instance;

        private static readonly object WebDriverDownloadingLock = new object();

        public static IWebDriver GetDriver()
        {
            var browserProfile = new BrowserProfile(Settings.GetSettings());
            return CreateDriver(browserProfile);
        }

        private static IWebDriver CreateDriver(IBrowserProfile browserProfile)
        {
            Logger.Info($"{browserProfile.BrowserName} driver initialization");
            switch (browserProfile.BrowserName)
            {
                case BrowserName.Chrome:
                    SetUpDriver(new ChromeConfig(), browserProfile.DriverSettings);
                    return new ChromeDriver((ChromeOptions) browserProfile.DriverSettings.DriverOptions);
                case BrowserName.Firefox:
                    SetUpDriver(new FirefoxConfig(), browserProfile.DriverSettings);
                    return new FirefoxDriver((FirefoxOptions)browserProfile.DriverSettings.DriverOptions);
                default:
                    throw new ArgumentOutOfRangeException($"Browser {browserProfile.BrowserName} is not supported.");
            }
        }

        private static void SetUpDriver(IDriverConfig driverConfig, IDriverSettings driverSettings)
        {
            var architecture = driverSettings.SystemArchitecture.Equals(Architecture.Auto) ? ArchitectureHelper.GetArchitecture() : driverSettings.SystemArchitecture;
            var version = driverSettings.WebDriverVersion.Equals("Latest") ? driverConfig.GetLatestVersion() : driverSettings.WebDriverVersion;
            var url = UrlHelper.BuildUrl(architecture.Equals(Architecture.X32) ? driverConfig.GetUrl32() : driverConfig.GetUrl64(), version);
            var binaryPath = FileHelper.GetBinDestination(driverConfig.GetName(), version, architecture, driverConfig.GetBinaryName());
            if (!File.Exists(binaryPath) || !Environment.GetEnvironmentVariable("PATH").Contains(binaryPath))
            {
                lock (WebDriverDownloadingLock)
                {
                    new DriverManager().SetUpDriver(url, binaryPath, driverConfig.GetBinaryName());
                }
            }
        }

    }
    public enum BrowserName
    {
        Chrome,
        Firefox
    }
}
