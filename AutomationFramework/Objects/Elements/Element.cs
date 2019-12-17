﻿using AutomationFramework.Browsers;
using OpenQA.Selenium;

namespace AutomationFramework.Objects.Elements
{
    public abstract class Element
    {
        private By selector;
        private string name;
        private IWebElement webElement;

        public Element(By selector, string name)
        {
            this.selector = selector;
            this.name = name;
            try
            {
                //log
                webElement = Browser.GetDriver().FindElement(selector);
            }
            catch (NoSuchElementException e)
            {
                //log
            }
        }

        public Element(IWebElement element)
        {
            webElement = element;
        }

        public bool IsDisplayed()
        {
            //log
            return webElement.Displayed;
        }

        public bool IsEnabled()
        {
            return webElement.Enabled;
        }

        public string GetText()
        {
            //log
            return webElement.Text;
        }

        public void Click()
        {
            webElement.Click();
        }
    }
}

