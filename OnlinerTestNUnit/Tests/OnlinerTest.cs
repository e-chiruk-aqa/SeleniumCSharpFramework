using System.Collections.Generic;
using AutomationFramework.Browsers;
using NUnit.Framework;
using OnlinerTestNUnit.Base;
using OnlinerTestNUnit.Forms;
using OnlinerTestNUnit.Forms.Catalog;

namespace OnlinerTestNUnit.Tests
{
    [TestFixture]
    public class OnlinerTest : BaseTest
    {

        [Test]
        public void SearchIPhoneInCatalog()
        {
            Step("Open onliner home page", () => 
                Browser.GetInstance().Navigate("http://www.onliner.by"));
            Step("Onliner home page is opened", () =>
                Assert.IsTrue(new OnlinerHome().WaitForDisplayed()));
            Step("Open catalog", () =>
                new HeaderForm().SelectTabByName("Каталог"));
            Step("Catalog page is opened", () =>
                Assert.IsTrue(new CatalogForm().WaitForDisplayed()));
            Step("Open mobile phones page from Catalog", () =>
                new CatalogForm().SelectCatalogBarItemByName("Мобильные телефоны"));
            Step("Mobile phones catalog page is opened", () =>
                Assert.IsTrue(new CatalogByNameForm("Мобильные телефоны").WaitForDisplayed()));
            Step("Open 'iPhone 11 64GB (черный)' product", () =>
            {
                var data = new Dictionary<string, List<string>>
                {
                    {"Производитель", new List<string> {"Apple"}}
                };
                new CatalogFilterForm().ApplyFilters(data);
                new CatalogProductsForm().SelectProduct("Смартфон Apple iPhone 11 64GB (черный)");
            });
            Step("'iPhone 11 64GB (черный)' product page is open", () => 
                Assert.AreEqual("Смартфон Apple iPhone 11 64GB (черный)", new CatalogProductForm().GetTitle()));
        }
    }
}
