using AutomationFramework.Elements;
using AutomationFramework.Forms;
using OpenQA.Selenium;

namespace OnlinerTestNUnitAllure.Forms.Catalog
{
    public class CatalogProductForm : BaseForm
    {
        private Label TitleLabel => new Label(By.ClassName("catalog-masthead__title"), "Title");

        public CatalogProductForm() : base(By.XPath("//div[contains(@class, 'product_details')]"), "Product")
        {
        }

        public string GetTitle()
        {
            return TitleLabel.GetText().Trim();
        }
    }
}