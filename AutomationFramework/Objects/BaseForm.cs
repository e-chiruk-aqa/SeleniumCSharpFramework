using AutomationFramework.Objects.Elements;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AutomationFramework.Objects
{
    public abstract class BaseForm
    {

        #region fields
        private readonly By formLocator;
        private string formName;
        #endregion

        protected BaseForm(By formLocator, string formName)
        {
            this.formLocator = formLocator;
            this.formName = formName;
            if (!IsFormOpen())
            {
                Assert.Fail($"{formName} form not found");
            }
        }

        public bool IsFormOpen()
        {
            var label = new Label(formLocator, $"Form with unique locator: {formLocator}");
            return label.IsDisplayed();
        }
    }
}
