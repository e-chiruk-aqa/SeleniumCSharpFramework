using AutomationFramework.Browsers;
using NUnit.Framework;
using OnlinerTestSpecflow.Extensions;
using OnlinerTestSpecflow.Forms;
using OnlinerTestSpecflow.Forms.Catalog;
using TechTalk.SpecFlow;

namespace OnlinerTestSpecflow.Steps
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
        public void CheckCatalogPageIsOpened()
        {
            Assert.IsTrue(new CatalogForm().WaitForDisplayed());
        }

        [When(@"I go to '(.*)' page from Catalog")]
        public void GoToPageFromCatalog(string pageName)
        {
            new CatalogForm().SelectCatalogBarItemByName(pageName);
        }

        [Then(@"'(.*)' catalog page is open")]
        public void CheckCatalogPageByNameIsOpen(string name)
        {
            Assert.IsTrue(new CatalogByNameForm(name).WaitForDisplayed());
        }

        [When(@"I apply filters on Catalog page:")]
        public void ApplyFiltersOnCatalogPage(Table table)
        {
            var data = table.ToDictionaryList();
            new CatalogFilterForm().ApplyFilters(data);
        }


        [When(@"I open '(.*)' product")]
        public void GoToProduct(string name)
        {
            new CatalogProductsForm().SelectProduct(name);
        }

        [Then(@"'(.*)' catalog product page is open")]
        public void CheckCatalogProductPageIsOpen(string product)
        {
            Assert.AreEqual(product, new CatalogProductForm().GetTitle(), "Unexpected product opened");
        }

    }
}
