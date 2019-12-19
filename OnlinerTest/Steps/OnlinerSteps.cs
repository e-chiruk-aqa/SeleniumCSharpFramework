using AutomationFramework;
using AutomationFramework.Browsers;
using NUnit.Framework;
using OnlinerTest.Forms;
using TechTalk.SpecFlow;

namespace OnlinerTest.Steps
{
    [Binding]
    class OnlinerSteps : BaseSteps
    {
        [Given(@"This url '(.*)' is opened")]
        public void NavigateTo(string url)
        {
            Browser.GetInstance().Navigate(url);
        }

        [When(@"I click '(.*)' tab")]
        public void ClickTab(string tab)
        {
            new HeaderForm().SelectTabByName(tab);
        }

        [Then(@"Catalog page is opened")]
        public void ThenCatalogPageIsOpened()
        {
            Assert.IsTrue(new CatalogForm().WaitForDisplayed());
        }

        [When(@"I select the '(.*)' filter by manufacturer on Mobile phones page")]
        public void WhenISelectTheFilterByManufacturerOnMobilePhonesPage(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I open '(.*)' phone")]
        public void WhenIOpenPhone(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"'(.*)' page is open")]
        public void ThenPageIsOpen(string p0)
        {
            ScenarioContext.Current.Pending();
        }

    }
}
