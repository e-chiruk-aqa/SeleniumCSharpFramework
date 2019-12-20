using AutomationFramework.Forms;
using OpenQA.Selenium;

namespace OnlinerTest.Forms.Catalog
{
    public class CatalogByNameForm : BaseForm
    {
        public CatalogByNameForm(string formName) : base(By.XPath($"//div[contains(@class, 'catalog-content')]//div[@class='schema-header']/h1[contains(text(), '{formName}')]"), formName)
        {
        }
    }
}