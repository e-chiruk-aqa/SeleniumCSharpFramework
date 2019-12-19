using OpenQA.Selenium;

namespace AutomationFramework.Elements
{
    public class Label : Element
    {
        public Label(By selector, string name, ElementState state) : base(selector, $"Label {name}", state)
        {
        }

        public Label(By selector, string name) : base(selector, $"Label {name}", ElementState.Exists)
        {
        }
    }
}
