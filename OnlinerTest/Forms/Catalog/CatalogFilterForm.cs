using AngleSharp.Common;
using AutomationFramework.Browsers;
using AutomationFramework.Elements;
using AutomationFramework.Forms;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace OnlinerTest.Forms.Catalog
{
    public class CatalogFilterForm : BaseForm
    {
        private CheckBox FilterCheckBox(string sectionName, string checkboxName) => new CheckBox(By.XPath($"//div[.//span[contains(text(), '{sectionName}')]]//ul//span[contains(text(), '{checkboxName}')]/preceding-sibling::span"), $"{sectionName} {checkboxName}");

        public CatalogFilterForm() : base(By.Id("schema-filter"), "Filter")
        {
        }

        public void ApplyFilters(Table data)
        {
            foreach (var row in data.Rows)
            {
                foreach (var sectionName in row.Keys)
                {
                    var checkbox = FilterCheckBox(sectionName, row.GetOrDefault(sectionName, string.Empty));
                    checkbox.ScrollToElement();
                    checkbox.Check();
                    Browser.GetInstance().WaitForPageToLoad();
                }
            }
        }
    }
}