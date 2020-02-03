using System.Collections.Generic;
using Allure.NUnit.Attributes;
using AutomationFramework.Browsers;
using AutomationFramework.Elements;
using AutomationFramework.Forms;
using OpenQA.Selenium;

namespace OnlinerTestNUnitAllure.Forms.Catalog
{
    public class CatalogFilterForm : BaseForm
    {
        private CheckBox FilterCheckBox(string sectionName, string checkboxName) => new CheckBox(By.XPath($"//div[.//span[contains(text(), '{sectionName}')]]//ul//span[contains(text(), '{checkboxName}')]/preceding-sibling::span"), $"{sectionName} {checkboxName}");

        public CatalogFilterForm() : base(By.Id("schema-filter"), "Filter")
        {
        }

        [AllureStep("I apply filters on Catalog page")]
        public void ApplyFilters(Dictionary<string, List<string>> data)
        {
            foreach (var sectionName in data.Keys)
            {
                foreach (var checkboxName in data.GetValueOrDefault(sectionName))
                {
                    var checkbox = FilterCheckBox(sectionName, checkboxName);
                    checkbox.ScrollToElement();
                    checkbox.Check();
                    Browser.GetInstance().WaitForPageToLoad();
                }
            }
        }
    }
}