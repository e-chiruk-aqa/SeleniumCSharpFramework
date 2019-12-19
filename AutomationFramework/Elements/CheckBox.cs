using AutomationFramework.Logging;
using OpenQA.Selenium;

namespace AutomationFramework.Elements
{
    public class CheckBox : Element
    {
        public CheckBox(By selector, string name, ElementState state) : base(selector, $"CheckBox {name}", state)
        {
        }

        public CheckBox(By selector, string name) : base(selector, $"CheckBox {name}", ElementState.Exists)
        {
        }

        public bool IsChecked
        {
            get
            {
                LogElementAction("Checking state");
                return GetElement().Selected;
            }
        }

        public void Check()
        {
            SetState(true);
        }

        public void Uncheck()
        {
            SetState(false);
        }

        private void SetState(bool state)
        {
            LogElementAction("Setting state", state);
            if (state != IsChecked)
            {
                Click();
            }
        }
    }
}