using System;
using OpenQA.Selenium;

namespace AutomationFramework.Elements
{
    public class ElementStates
    {
        public ElementStates(Func<IWebElement, bool> elementStateCondition, string stateName)
        {
            ElementStateCondition = elementStateCondition;
            StateName = stateName;
        }

        public Func<IWebElement, bool> ElementStateCondition { get; }

        public bool IsCatchingTimeoutException { get; set; }

        public bool IsThrowingNoSuchElementException { get; set; }

        public string StateName { get; }
    }

    public enum ElementState
    {
        Displayed,
        Exists
    }
}
