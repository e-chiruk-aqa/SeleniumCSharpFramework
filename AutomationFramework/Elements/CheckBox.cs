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
                LogElementAction("Getting state");
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
            LogElementAction("Setting state '{0}'", state);
            if (state != IsChecked)
            {
                Click();
            }
        }
    }
}