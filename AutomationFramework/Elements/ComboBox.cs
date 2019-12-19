using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationFramework.Elements
{
    public class ComboBox : Element
    {
        public ComboBox(By selector, string name, ElementState state) : base(selector, $"ComboBox {name}", state)
        {
        }
        public ComboBox(By selector, string name) : base(selector, $"ComboBox {name}", ElementState.Exists)
        {
        }

        public void SelectByText(string text)
        {
            LogElementAction("Selecting by text");
            new SelectElement(GetElement()).SelectByText(text);
        }

        public void SelectByValue(string value)
        {
            LogElementAction("Selecting by value");
            new SelectElement(GetElement()).SelectByValue(value);
        }

        public string SelectedValue => new SelectElement(GetElement()).SelectedOption.GetAttribute("value");

        public string SelectedText => new SelectElement(GetElement()).SelectedOption.Text;
    }
}
