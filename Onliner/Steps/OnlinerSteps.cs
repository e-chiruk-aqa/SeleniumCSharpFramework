using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Selenium;
using TechTalk.SpecFlow;

namespace Onliner.Steps
{
    [Binding]
    class OnlinerSteps
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
