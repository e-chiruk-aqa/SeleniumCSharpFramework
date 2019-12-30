using AutomationFramework.Browsers;
using AutomationFramework.Elements;
using AutomationFramework.Forms;
using NUnit.Allure.Steps;
using OpenQA.Selenium;

namespace OnlinerTestNUnitAllure.Forms.Catalog
{
    public class CatalogProductsForm : BaseForm
    {
        private Button ProductByName(string name) => new Button(By.XPath($"//div[@class='schema-product__title']//span[contains(text(), '{name}')]"), $"Product {name}");

        private Button NextPage => new Button(By.Id("schema-pagination"), "Next Page");

        public CatalogProductsForm() : base(By.Id("schema-products"), "Products")
        {
        }

        [AllureStep("Open &name& product")]
        public void SelectProduct(string name)
        {
            var product = ProductByName(name);
            while (!product.IsExists())
            {
                NextPage.ScrollToElement();
                NextPage.Click();
                Browser.GetInstance().WaitForPageToLoad();
            }
            product.WaitForDisplayed();
            product.ScrollToElement();
            product.Click();
        }
    }
}