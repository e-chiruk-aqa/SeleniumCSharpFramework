using OpenQA.Selenium;

namespace AutomationFramework.Elements
{
    public class Button : Element
    {
        public Button(By selector, string name, ElementState state) : base(selector, $"Button {name}", state)
        {
        }

        public Button(By selector, string name) : base(selector, $"Button {name}", ElementState.Exists)
        {
        }
    }
}
