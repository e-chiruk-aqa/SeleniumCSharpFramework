using OpenQA.Selenium;

namespace AutomationFramework.Objects.Elements
{
    public class Label : Element
    {
        public Label(By selector, string name) : base(selector, name)
        {
        }

        private Label(IWebElement element) : base(element)
        {
        }
    }
}
