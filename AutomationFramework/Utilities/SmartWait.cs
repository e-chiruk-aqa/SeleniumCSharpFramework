using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AutomationFramework.Browsers;
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

        public static void WaitForTrue(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string message = null)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "condition cannot be null");
            }

            var waitTimeout = ResolveConditionTimeout(timeout);
            var checkInterval = ResolvePollingInterval(pollingInterval);
            var stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (condition())
                {
                    return;
                }

                if (stopwatch.Elapsed > waitTimeout)
                {
                    var exceptionMessage = $"Timed out after {waitTimeout.TotalSeconds} seconds";
                    if (!string.IsNullOrEmpty(message))
                    {
                        exceptionMessage += $": {message}";
                    }

                    throw new TimeoutException(exceptionMessage);
                }
                Thread.Sleep(checkInterval);
            }
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
