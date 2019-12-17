using System;
using System.Collections.Generic;
using AutomationFramework.Browsers;
using AutomationFramework.Utilities;
using OpenQA.Selenium;

namespace AutomationFramework.Configuration.WebDriverSettings
{
    public class FirefoxSettings : DriverSettings
    {
        public FirefoxSettings(ISettingsFile settingsFile) : base(settingsFile)
        {
        }

        protected override BrowserName BrowserName => BrowserName.Firefox;

        protected override IDictionary<string, Action<DriverOptions, object>> KnownCapabilitySetters => new Dictionary<string, Action<DriverOptions, object>>
        {
            //TODO
        };

        public override DriverOptions DriverOptions { get; }

        public override string DownloadDirCapabilityKey => "browser.download.dir";
        
    }
}
