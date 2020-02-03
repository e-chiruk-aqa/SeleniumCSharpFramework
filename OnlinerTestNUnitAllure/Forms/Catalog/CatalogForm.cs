using Allure.NUnit.Attributes;
using AutomationFramework.Elements;
using AutomationFramework.Forms;
using OpenQA.Selenium;

namespace OnlinerTestNUnitAllure.Forms.Catalog
{
    public class CatalogForm : BaseForm
    {
        private Button CatalogBarItemByName(string name) => new Button(By.XPath($"//div[@class='catalog-bar']//a[text()='{name}']"), $"Catalog bar item {name}");

        public CatalogForm() : base(By.XPath("//div[contains(@class, 'catalog-navigation_opened')]"), "Catalog")
        {
        }

        [AllureStep("Open &name& page from Catalog")]
        public void SelectCatalogBarItemByName(string name)
        {
            CatalogBarItemByName(name).Click();
        }
    }
}