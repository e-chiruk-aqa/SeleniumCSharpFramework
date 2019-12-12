using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using OpenQA.Selenium;

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

        public void windowMaximize()
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
