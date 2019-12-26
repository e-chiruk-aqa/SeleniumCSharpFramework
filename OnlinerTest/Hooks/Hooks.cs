using System;
using System.Drawing;
using System.IO;
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
                AllureLifecycle.Instance.AddAttachment("Screenshot", "image/png", MakeScreenshot());
            });
        }

        [AfterScenario]
        public void AfterScenarioSteps()
        {
            if (_scenarioContext.TestError != null)
            {
                foreach (var failedScreen in Directory.GetFiles(FileProvider.GetFailedScreensDirectory()))
                {
                    _allureLifecycle.AddAttachment(failedScreen);
                }
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
