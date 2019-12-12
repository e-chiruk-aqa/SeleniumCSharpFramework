using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Framework.Utils
{
    public class SmartWait
    {
        public static void waitFor(Func<IWebDriver, bool> condition, long timeOutInSeconds)
        {
            Browser.GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
            var wait = new DefaultWait<IWebDriver>(Browser.GetDriver())
            {
                Timeout = TimeSpan.FromSeconds(timeOutInSeconds), 
                PollingInterval = TimeSpan.FromMilliseconds(300)
            };
            try
            {
                return wait.Until()
            }
            catch (Exception | AssertionError e) {
                logger.debug("SmartWait.waitFor", e);
            } finally
            {
                Browser.getDriver().manage().timeouts().implicitlyWait(Long.parseLong(Browser.getTimeoutForCondition()), TimeUnit.SECONDS);
            }
            return null;
        }
    }
}
