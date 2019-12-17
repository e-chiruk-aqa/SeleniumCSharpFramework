using AutomationFramework;
using AutomationFramework.Browsers;
using TechTalk.SpecFlow;

namespace OnlinerTest.Steps
{
    [Binding]
    class OnlinerSteps : BaseSteps
    {
        [Given(@"This url '(.*)' is opened")]
        public void GivenThisUrlIsOpened(string url)
        {
            Browser.GetInstance().Navigate(url);
        }

        [When(@"I click Catalog tab")]
        public void WhenIClickCatalogTab()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"Catalog page is opened")]
        public void ThenCatalogPageIsOpened()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
