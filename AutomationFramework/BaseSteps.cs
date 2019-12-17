using NLog;
using NUnit.Allure.Core;

namespace AutomationFramework
{
    [AllureNUnit()]
    public class BaseSteps
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
    }
}