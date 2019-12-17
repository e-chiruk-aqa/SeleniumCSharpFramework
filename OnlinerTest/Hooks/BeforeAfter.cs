using AutomationFramework.Browsers;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace OnlinerTest.Hooks
{
    [Binding]
    public sealed class BeforeAfter
    {
        [BeforeScenario()]
        public static void Before()
        {
            Browser.GetInstance().WindowMaximize();
            Assert.IsTrue(true);
        }

        [AfterScenario()]
        public static void After()
        {
            Browser.GetInstance().Exit();
        }
    }
}
