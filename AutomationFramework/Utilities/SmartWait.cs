using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationFramework.Browsers;
using AutomationFramework.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationFramework.Utilities
{
    public static class SmartWait
    {
        public static T WaitFor<T>(Func<IWebDriver, T> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, IList<Type> exceptionsToIgnore = null)
        {
            Browser.GetInstance().SetImplicitWaitTimeout(TimeSpan.Zero);
            var waitTimeout = ResolveConditionTimeout(timeout);
            var checkInterval = ResolvePollingInterval(pollingInterval);
            var wait = new WebDriverWait(Browser.GetInstance().GetDriver(), waitTimeout)
            {
                PollingInterval = checkInterval
            };
            var ignoreExceptions = exceptionsToIgnore ?? new List<Type> { typeof(StaleElementReferenceException) };
            wait.IgnoreExceptionTypes(ignoreExceptions.ToArray());
            var result = wait.Until(condition);
            Browser.GetInstance().SetImplicitWaitTimeout(Browser.ImplicitWaitTimeout);
            return result;
        }

        private static TimeSpan ResolveConditionTimeout(TimeSpan? timeout)
        {
            return timeout ?? Browser.ConditionTimeout;
        }

        private static TimeSpan ResolvePollingInterval(TimeSpan? pollingInterval)
        {
            return pollingInterval ?? Browser.PollingInterval;
        }
    }
}
