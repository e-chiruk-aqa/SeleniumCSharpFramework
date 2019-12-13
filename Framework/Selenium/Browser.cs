using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Framework.Selenium
{
    public sealed class Browser
    {
        private static IWebDriver webDriver;
        private static Browser instance { get; set; }
        private static string timeOutForPageLoad { get; set; }

        private Browser()
        {
            //log
        }

        private static void InitProperties()
        {
            timeOutForPageLoad = ConfigurationManager.AppSettings["DefaultPageLoadTimeout"];
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
            var driver = BrowserFactory.GetDriver(ConfigurationManager.AppSettings["Browser"]);
            driver.Manage().Timeouts().ImplicitWait =
                TimeSpan.FromSeconds(double.Parse(ConfigurationManager.AppSettings["DefaultImplicitWait"]));
            return driver;
        }

        public void Exit()
        {
            try
            {
                GetDriver().Quit();
            }
            catch (Exception e)
            {
                //log
            }
            finally
            {
                webDriver = null;
            }
        }

        public static IWebDriver GetDriver()
        {
            return webDriver ?? (webDriver = GetNewDriver());
        }

        public void WaitForPageToLoad()
        {
            Func<IWebDriver, bool> condition = driver =>
            {
                var result =
                    ((IJavaScriptExecutor) driver).ExecuteScript(
                        "return document['readyState'] ? 'complete' == document.readyState : true");
                return result is bool && (bool) result;
            };
            var wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(double.Parse(timeOutForPageLoad)));
            wait.Until(condition);
        }

        public void WindowMaximize()
        {
            GetDriver().Manage().Window.Maximize();
        }

        public void Refresh()
        {
            GetDriver().Navigate().Refresh();
        }

        public void Back()
        {
            GetDriver().Navigate().Back();
        }

        public void Navigate(string url)
        {
            GetDriver().Navigate().GoToUrl(url);
        }
    }
}
