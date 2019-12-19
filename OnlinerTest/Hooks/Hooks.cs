using System;
using Allure.Commons;
using AutomationFramework.Browsers;
using AutomationFramework.Logging;
using AutomationFramework.Utilities;
using TechTalk.SpecFlow;

namespace OnlinerTest.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        private AllureLifecycle _allureLifecycle;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _allureLifecycle = AllureLifecycle.Instance;
        }

        [BeforeScenario()]
        public void Before()
        {
            Browser.GetInstance().WindowMaximize();
            AllureLifecycle.Instance.SetCurrentTestActionInException(() =>
            {
                var arr = Browser.GetInstance().GetScreenshot();
                AllureLifecycle.Instance.AddAttachment("Screenshot", AllureLifecycle.AttachFormat.ImagePng, arr);
            });
        }

        [AfterScenario]
        public void AfterScenarioSteps()
        {
            if (_scenarioContext.TestError != null)
            {
                _allureLifecycle.AddAttachment(MakeScreenshot());
            }
            Browser.GetInstance().Quit();
            _allureLifecycle.AddAttachment(Logger.GetLogLocation());

        }

        private string MakeScreenshot()
        {
            return ScreenshotProvider.PublishScreenshot($"Screenshot_{DateTime.Now.ToFileTime()}");
        }

    }

}
