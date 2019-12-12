using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Framework.Selenium
{
    public class BrowserFactory
    {
        public static IWebDriver GetDriver(string type)
        {
            foreach (var browser in Enum.GetNames(typeof(Browsers)))
            {
                if (string.Equals(browser, type, StringComparison.OrdinalIgnoreCase))
                {
                    return GetDriver((Browsers) Enum.Parse(typeof(Browsers), browser));
                }
            }
            throw new Exception($"Unknown browser type - {type}");
        }

        private static IWebDriver GetDriver(Browsers type)
        {
            switch (type)
            {
                case Browsers.Chrome:
                    return GetChromeDriver();
                case Browsers.Firefox:
                    return GetFirefoxDriver();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static FirefoxOptions GetFirefoxOptions()
        {
            var firefoxOptions = new FirefoxOptions { AcceptInsecureCertificates = true };
            return firefoxOptions;
        }

        private static IWebDriver GetFirefoxDriver()
        {
            return new FirefoxDriver(GetFirefoxOptions());
        }

        private static ChromeOptions GetChromeOptions()
        {
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("start-maximized");
            return option;
        }

        private static IWebDriver GetChromeDriver()
        {
            return new ChromeDriver(GetChromeOptions());
        }

        public enum Browsers
        {
            Chrome,
            Firefox
        }
    }


}
