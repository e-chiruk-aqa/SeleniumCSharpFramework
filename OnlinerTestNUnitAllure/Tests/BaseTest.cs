using System;
using System.IO;
using Allure.Commons;
using AutomationFramework.Browsers;
using AutomationFramework.Utilities;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace OnlinerTestNUnitAllure.Tests
{
    public class BaseTest : AllureReport
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            AllureLifecycle.Instance.CleanupResultDirectory();
        }

        [SetUp]
        public void Setup()
        {
            Browser.GetInstance().WindowMaximize();
            FileProvider.CleanDirectory(FileProvider.GetFailedScreensDirectory());
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                foreach (var failedScreen in Directory.GetFiles(FileProvider.GetFailedScreensDirectory()))
                {
                    AllureLifecycle.Instance.AddAttachment(failedScreen);
                }
                AllureLifecycle.Instance.AddAttachment("Screenshot", "image/png",
                    ScreenshotProvider.PublishScreenshot($"Screenshot_{DateTime.Now.ToFileTime()}"));
            }
            Browser.GetInstance().Quit();
        }
    }
}