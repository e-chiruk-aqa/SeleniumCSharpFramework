using OpenQA.Selenium;

namespace AutomationFramework.Elements
{
    public class TextBox : Element
    {
        private const string SecretMask = "*********";

        public TextBox(By selector, string name, ElementState state) : base(selector, $"TextBox {name}", state)
        {
        }
        public TextBox(By selector, string name) : base(selector, $"TextBox {name}", ElementState.Exists)
        {
        }

        public string Value => GetElement().GetAttribute("value");

        public void Type(string value, bool secret = false)
        {
            LogElementAction("Typing '{0}'", secret ? SecretMask : value);
            GetElement().SendKeys(value);
        }

        public void ClearAndType(string value, bool secret = false)
        {
            LogElementAction("Typing '{0}'", secret ? SecretMask : value);
            GetElement().Clear();
            GetElement().SendKeys(value);
        }

        public void Submit()
        {
            GetElement().Submit();
        }
    }
}