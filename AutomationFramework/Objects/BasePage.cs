using System;
using AutomationFramework.Browsers;
using AutomationFramework.Objects.Elements;
using OpenQA.Selenium;

namespace AutomationFramework.Objects
{
    public abstract class BasePage
    {
        public By UniqueLocator;

        public BasePage(By uniqueLocator)
        {
            WaitForPageToLoad();
            UniqueLocator = uniqueLocator;
            if (!IsPageOpen())
            {
                //log
                throw new Exception();
            }
        }

        public static void WaitForPageToLoad()
        {
            Browser.GetInstance().WaitForPageToLoad();
        }

        public bool IsPageOpen()
        {
            var label = new Label(UniqueLocator, $"Page with unique locator: {UniqueLocator}");
            return label.IsDisplayed();
        }
    }
}
