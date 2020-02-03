using Allure.NUnit.Attributes;
using AutomationFramework.Elements;
using AutomationFramework.Forms;
using OpenQA.Selenium;

namespace OnlinerTestNUnitAllure.Forms
{
    public class HeaderForm : BaseForm
    {
        private Button TabByName(string name) => new Button(By.XPath($"//span[@class='b-main-navigation__text' and text()='{name}']"), $"{name} tab");
        public HeaderForm() : base(By.XPath("//header[@class='g-top']"), "Header")
        {
        }

        [AllureStep("Open &name& header tab")]
        public void SelectTabByName(string name)
        {
            WaitForDisplayed();
            TabByName(name).Click();
        }
    }
}