﻿using AutomationFramework.Browsers;
using AutomationFramework.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationFramework.Configuration.WebDriverSettings
{
    public class ChromeSettings : DriverSettings
    {
        public ChromeSettings(ISettingsFile settingsFile) : base(settingsFile)
        {
        }

        protected override BrowserName BrowserName => BrowserName.Chrome;

        public override string DownloadDirCapabilityKey => "download.default_directory";

        public override DriverOptions DriverOptions
        {
            get
            {
                var options = new ChromeOptions();
                SetChromePrefs(options);
                SetCapabilities(options, (name, value) => options.AddAdditionalCapability(name, value, true));
                SetChromeArguments(options);
                return options;
            }
        }

        private void SetChromePrefs(ChromeOptions options)
        {
            foreach (var option in BrowserOptions)
            {
                var value = option.Key == DownloadDirCapabilityKey ? DownloadDir : option.Value;
                options.AddUserProfilePreference(option.Key, value);
            }
        }

        private void SetChromeArguments(ChromeOptions options)
        {
            options.AddArguments(BrowserStartArguments);
        }
    }
}
