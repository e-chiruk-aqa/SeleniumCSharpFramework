using System.Collections.Generic;
using AutomationFramework.Browsers;
using AutomationFramework.Forms;
using NUnit.Allure.Core;
using NUnit.Allure.Steps;
using NUnit.Framework;
using OnlinerTestNUnitAllure.Forms;
using OnlinerTestNUnitAllure.Forms.Catalog;

namespace OnlinerTestNUnitAllure.Tests
{
    [TestFixture]
    [AllureNUnit]
    public class OnlinerTest : BaseTest
    {

        [Test]
        [Description("Search Iphone in catalog with applying filters")]
        public void SearchIPhoneInCatalog()
        {
            OpenUrl("https://www.onliner.by");
            CheckPageOpened(new OnlinerHome(), "Onliner Home");
            new HeaderForm().SelectTabByName("�������");
            CheckPageOpened(new CatalogForm(), "Catalog");
            new CatalogForm().SelectCatalogBarItemByName("��������� ��������");
            CheckPageOpened(new CatalogByNameForm("��������� ��������"), "Mobile phones");
            var data = new Dictionary<string, List<string>>
            {
                {"�������������", new List<string> {"Apple"}}
            };
            new CatalogFilterForm().ApplyFilters(data);
            new CatalogProductsForm().SelectProduct("�������� Apple iPhone 11 64GB (������)");
            CheckProductTitle("�������� Apple iPhone 11 64GB (������)");

        }

        [AllureStep("&name& product page is open")]
        public void CheckProductTitle(string name)
        {
            Assert.AreEqual(name, new CatalogProductForm().GetTitle());
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