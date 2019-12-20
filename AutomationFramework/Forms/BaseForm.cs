using System.Linq;
using AutomationFramework.Browsers;
using AutomationFramework.Elements;
using AutomationFramework.Utilities;
using OpenQA.Selenium;

namespace AutomationFramework.Forms
{
    public abstract class BaseForm
    {
        private readonly By formSelector;
        private readonly string formName;

        protected BaseForm(By formSelector, string formName)
        {
            this.formSelector = formSelector;
            this.formName = $"{formName} form";
            Browser.GetInstance().WaitForPageToLoad();
        }

        public bool WaitForDisplayed()
        {
            return SmartWait.WaitFor(d => FormLabel.FindElements(formSelector, ElementState.Displayed).Any());
        }

        public bool IsDisplayed => FormLabel.IsDisplayed();

        private Label FormLabel => new Label(formSelector, formName);
    }
}
