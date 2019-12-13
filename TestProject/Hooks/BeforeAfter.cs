using Framework.Selenium;
using TechTalk.SpecFlow;

namespace Onliner.Hooks
{
    [Binding]
    public sealed class BeforeAfter
    {
        [BeforeTestRun]
        public static void Before()
        {
            Browser.GetInstance().WindowMaximize();
        }

        [AfterTestRun]
        public static void After()
        {
            Browser.GetInstance().Exit();
        }
    }
}