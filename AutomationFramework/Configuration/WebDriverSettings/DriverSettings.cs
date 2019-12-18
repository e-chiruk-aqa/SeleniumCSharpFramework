using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutomationFramework.Browsers;
using AutomationFramework.Utilities;
using OpenQA.Selenium;
using WebDriverManager.Helpers;

namespace AutomationFramework.Configuration.WebDriverSettings
{
    public abstract class DriverSettings : IDriverSettings
    {
        protected DriverSettings(ISettingsFile settingsFile)
        {
            SettingsFile = settingsFile;
        }

        private string DriverSettingsPath => $".driverSettings.{BrowserName.ToString().ToLowerInvariant()}";

        protected IDictionary<string, object> BrowserCapabilities => SettingsFile.GetValueOrNew<Dictionary<string, object>>($"{DriverSettingsPath}.capabilities");

        protected IDictionary<string, object> BrowserOptions => SettingsFile.GetValueOrNew<Dictionary<string, object>>($"{DriverSettingsPath}.options");

        protected IReadOnlyList<string> BrowserStartArguments => SettingsFile.GetValueListOrEmpty<string>($"{DriverSettingsPath}.startArguments");

        protected ISettingsFile SettingsFile { get; }

        protected abstract BrowserName BrowserName { get; }

        protected virtual IDictionary<string, Action<DriverOptions, object>> KnownCapabilitySetters => new Dictionary<string, Action<DriverOptions, object>>();

        public abstract string DownloadDirCapabilityKey { get; }

        public abstract DriverOptions DriverOptions { get; }

        public string WebDriverVersion => SettingsFile.GetValueOrDefault($"{DriverSettingsPath}.webDriverVersion", defaultValue: "Latest");

        public Architecture SystemArchitecture => SettingsFile.GetValueOrDefault($"{DriverSettingsPath}.systemArchitecture", "Auto").ToEnum<Architecture>();

        public virtual string DownloadDir
        {
            get
            {
                if (BrowserOptions.ContainsKey(DownloadDirCapabilityKey))
                {
                    var pathInConfiguration = BrowserOptions[DownloadDirCapabilityKey].ToString();
                    return pathInConfiguration.Contains(".") ? Path.GetFullPath(pathInConfiguration) : pathInConfiguration;
                }

                throw new InvalidOperationException($"Failed to find {DownloadDirCapabilityKey} option in settings profile for {BrowserName}");
            }
        }

        protected void SetCapabilities(DriverOptions options, Action<string, object> addCapabilityMethod = null)
        {
            foreach (var capability in BrowserCapabilities)
            {
                try
                {
                    var defaultAddCapabilityMethod = addCapabilityMethod ?? options.AddAdditionalCapability;
                    defaultAddCapabilityMethod(capability.Key, capability.Value);
                }
                catch (ArgumentException exception)
                {
                    if (exception.Message.StartsWith("There is already an option"))
                    {
                        SetKnownProperty(options, capability, exception);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void SetKnownProperty(DriverOptions options, KeyValuePair<string, object> capability, ArgumentException exception)
        {
            if (KnownCapabilitySetters.ContainsKey(capability.Key))
            {
                KnownCapabilitySetters[capability.Key](options, capability.Value);
            }
            else
            {
                SetOptionByPropertyName(options, capability, exception);
            }
        }

        private void SetOptionByPropertyName(DriverOptions options, KeyValuePair<string, object> option, Exception exception)
        {
            var optionProperty = options
                            .GetType()
                            .GetProperties()
                            .FirstOrDefault(property => IsPropertyNameMatchOption(property.Name, option.Key) && property.CanWrite)
                            ?? throw exception;
            optionProperty.SetValue(options, option.Value);
        }

        private bool IsPropertyNameMatchOption(string propertyName, string optionKey)
        {
            return propertyName.Equals(optionKey, StringComparison.InvariantCultureIgnoreCase)
                || optionKey.ToLowerInvariant().Contains(propertyName.ToLowerInvariant());
        }
    }
}
