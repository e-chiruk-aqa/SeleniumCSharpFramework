using System;
using AutomationFramework.Configuration;
using AutomationFramework.Logging;
using AutomationFramework.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationFramework.Browsers
{
    public sealed class Browser
    {
        private static IWebDriver webDriver;

        private static Browser instance { get; set; }

        public static TimeSpan PageLoadTimeout { get; private set; }

        public static TimeSpan ImplicitWaitTimeout { get; private set; }

        public static TimeSpan ConditionTimeout { get; private set; }

        public static TimeSpan PollingInterval { get; private set; }

        public static ISettingsFile SettingsFile { get; private set; }

        public static Logger Logger = Logger.Instance;

        private Browser() { }

        private static void InitProperties()
        {
            Logger.Info("Initialization driver properties");
            SettingsFile = Settings.GetSettings();
            PageLoadTimeout = TimeSpan.FromSeconds(double.Parse(SettingsFile.GetValue<string>(".timeouts.timeoutPageLoad")));
            ImplicitWaitTimeout = TimeSpan.FromSeconds(double.Parse(SettingsFile.GetValue<string>(".timeouts.timeoutImplicit")));
            ConditionTimeout = TimeSpan.FromSeconds(double.Parse(SettingsFile.GetValue<string>(".timeouts.timeoutCondition")));
            webDriver = GetNewDriver();
        }

        public static Browser GetInstance()
        {
            if (instance != null) return instance;
            InitProperties();
            instance = new Browser();
            return instance;
        }

        private static IWebDriver GetNewDriver()
        {
            Logger.Info("Creating new driver");
            var driver = BrowserFactory.GetDriver();
            driver.Manage().Timeouts().ImplicitWait = ImplicitWaitTimeout;
            driver.Manage().Timeouts().PageLoad = PageLoadTimeout;
            return driver;
        }

        public IWebDriver GetDriver()
        {
            return webDriver ?? (webDriver = GetNewDriver());
        }

        public void WaitForPageToLoad()
        {
            Func<IWebDriver, bool> jsLoad = driver =>
            {
                var result =
                    ((IJavaScriptExecutor) driver).ExecuteScript(
                        "return document['readyState'] ? 'complete' == document.readyState : true");
                return result is bool && (bool) result;
            };
            Func<IWebDriver, bool> jQueryLoad = driver =>
            {
                var result =
                    ((IJavaScriptExecutor)driver).ExecuteScript(
                        "return jQuery.active == 0");
                return result is bool && (bool)result;
            };
            var wait = new WebDriverWait(GetDriver(), PageLoadTimeout);
            try
            {
                wait.Until(jQueryLoad);
            }
            catch (WebDriverException exception)
            {
                Logger.Debug("No jquery", exception);
            }
            wait.Until(jsLoad);
        }

        public void SetImplicitWaitTimeout(TimeSpan timeout)
        {
            GetDriver().Manage().Timeouts().ImplicitWait = timeout;
        }

        public void Scroll(int pixels)
        {
            ((IJavaScriptExecutor) GetDriver()).ExecuteScript($"window.scrollBy(0,{pixels})");
        }

        public void Quit()
        {
            Logger.Info("Closing browser");
            GetDriver().Quit();
            webDriver = null;
        }

        public void WindowMaximize()
        {
            Logger.Info("Window maximizing ");
            GetDriver().Manage().Window.Maximize();
        }

        public void Refresh()
        {
            Logger.Info("Refreshing the page");
            GetDriver().Navigate().Refresh();
        }

        public void Back()
        {
            Logger.Info("Return to previous page");
            GetDriver().Navigate().Back();
        }

        public void Navigate(string url)
        {
            Logger.Info($"Navigate to url - '{url}'");
            GetDriver().Navigate().GoToUrl(url);
        }

        public byte[] GetScreenshot()
        {
            return ((ITakesScreenshot) GetDriver()).GetScreenshot().AsByteArray;
        }
    }
}
