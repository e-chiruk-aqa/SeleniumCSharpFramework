using Allure.Commons;
using AutomationFramework.Browsers;
using NUnit.Framework;

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
        }

        [TearDown]
        public void TearDown()
        {
            Browser.GetInstance().Quit();
        }
    }
}