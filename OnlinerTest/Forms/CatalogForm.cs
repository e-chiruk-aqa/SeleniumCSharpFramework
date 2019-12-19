using AutomationFramework.Forms;
using OpenQA.Selenium;

namespace OnlinerTest.Forms
{
    public class CatalogForm : BaseForm
    {
        public CatalogForm() : base(By.XPath("//div[contains(@class, 'catalog-navigation_opened')]"), "Catalog")
        {
        }
    }
}