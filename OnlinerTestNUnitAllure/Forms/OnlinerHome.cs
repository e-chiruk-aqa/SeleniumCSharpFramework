using AutomationFramework.Forms;
using OpenQA.Selenium;

namespace OnlinerTestNUnitAllure.Forms
{
    public class OnlinerHome : BaseForm
    {
        public OnlinerHome() : base(By.XPath("//input[@data-project='onliner_main']"), "Onliner Nome")
        {
        }
    }
}
