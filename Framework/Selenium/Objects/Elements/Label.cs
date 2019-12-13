using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Framework.Selenium.Objects.Elements
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
