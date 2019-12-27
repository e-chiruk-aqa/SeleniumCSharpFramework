using Allure.NUnit.Attributes;
using AutomationFramework.Browsers;
using AutomationFramework.Forms;
using NUnit.Framework;
using OnlinerTestSpecflow.Forms;

namespace OnlinerTestNUnitAllure.Tests
{
    [TestFixture]
    public class OnlinerTest : BaseTest
    {

        [Test]
        [Description("Search Iphone in catalog with applying filters")]
        public void SearchIPhoneInCatalog()
        {
            OpenUrl("https://www.onliner.by");
            CheckPageOpened(new OnlinerHome(), "Onliner Home");
        }

        [AllureStep("Open this &url& url")]
        public void OpenUrl(string url)
        {
            Browser.GetInstance().Navigate(url);
        }

        [AllureStep("Check page &pageName& is opened")]
        public void CheckPageOpened<T>(T page, string pageName) where T : BaseForm
        {
            Assert.IsTrue(page.WaitForDisplayed());
        }
    }
}