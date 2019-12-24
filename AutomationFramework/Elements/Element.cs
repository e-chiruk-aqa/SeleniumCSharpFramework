using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using AShotNet;
using AShotNet.Coordinates;
using AShotNet.Cropper.Indent;
using AShotNet.ScreenTaker;
using AutomationFramework.Browsers;
using AutomationFramework.Logging;
using AutomationFramework.Utilities;
using OpenQA.Selenium;

namespace AutomationFramework.Elements
{
    public abstract class Element
    {
        private readonly By selector;
        private readonly string name;
        private readonly ElementState state;

        private delegate void Action();

        public Element(By selector, string name, ElementState state)
        {
            this.selector = selector;
            this.name = name;
            this.state = state;
        }

        public IWebElement GetElement()
        {
            try
            {
                Logger.Instance.Debug($"Find element with selector: {selector}");
                return FindElement(selector, state);
            }
            catch (NoSuchElementException)
            {
                Logger.Instance.Error($"Element with this {selector} selector not found");
                throw;
            }
        }

        public bool IsExists()
        {
            LogElementAction("Check existence");
            return FindElements(selector, timeout: TimeSpan.Zero).Any();
        }

        public bool IsDisplayed()
        {
            LogElementAction("Check is displayed");
            return GetElement().Displayed;
        }

        public bool IsEnabled()
        {
            LogElementAction("Check is enabled");
            return GetElement().Enabled;
        }

        public string GetText()
        {
            LogElementAction("Getting text");
            return GetElement().Text;
        }

        public void ClickViaJs()
        {
            LogElementAction("Clicking via js");
            ((IJavaScriptExecutor)Browser.GetInstance().GetDriver()).ExecuteScript(
                "arguments[0].click();", GetElement());
        }

        public void Click()
        {
            LogElementAction("Clicking");
            Action click = () => GetElement().Click();
            HandleElementException(click);
        }

        public void SendKeys(string key)
        {
            LogElementAction("Sending keys");
            GetElement().SendKeys(key);
        }

        public void SaveScreenshotWithHighlightedElement(string filename)
        {
            var driver = Browser.GetInstance().GetDriver();
            var js = (IJavaScriptExecutor) driver;
            var element = GetElement();
            var bgcolor = element.GetCssValue("backgroundColor");
            js.ExecuteScript("arguments[0].style.backgroundColor = '" + "red" + "'", element);
            new AShot().ShootingStrategy(new ViewportPastingStrategy(100))
                .TakeScreenshot(driver)
                .getImage()
                .Save(Path.Combine(FileProvider.GetOutputDirectory(), $"{filename}_{DateTime.Now.ToFileTime()}.png"));
            js.ExecuteScript("arguments[0].style.backgroundColor = '" + bgcolor + "'", element);
        }

        public void ScrollToElement()
        {
            LogElementAction("Scrolling to");
            ((IJavaScriptExecutor) Browser.GetInstance().GetDriver()).ExecuteScript(
                "arguments[0].scrollIntoView(true);", GetElement());
            WaitForDisplayed();
        }

        public void WaitForDisplayed()
        {
            SmartWait.WaitFor(d => IsDisplayed());
        }

        public IWebElement FindElement(By selector, ElementState state = ElementState.Exists, TimeSpan? timeout = null)
        {
            var elementStates = ResolveState(state);
            return FindElement(selector, elementStates.ElementStateCondition, elementStates.StateName, timeout);
        }

        public IWebElement FindElement(By selector, Func<IWebElement, bool> elementStateCondition, string stateName, TimeSpan? timeout = null)
        {
            var desiredState = new ElementStates(elementStateCondition, stateName)
            {
                IsCatchingTimeoutException = false,
                IsThrowingNoSuchElementException = true
            };
            return FindElements(selector, desiredState, timeout).First();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By selector, ElementState state = ElementState.Exists, TimeSpan? timeout = null)
        {
            var elementStateCondition = ResolveState(state);
            elementStateCondition.IsCatchingTimeoutException = true;
            return FindElements(selector, elementStateCondition, timeout);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By selector, ElementStates elementState, TimeSpan? timeout = null)
        {
            var foundElements = new List<IWebElement>();
            var resultElements = new List<IWebElement>();
            try
            {
                SmartWait.WaitFor(driver =>
                {
                    foundElements = driver.FindElements(selector).ToList();
                    resultElements = foundElements.Where(elementState.ElementStateCondition).ToList();
                    return resultElements.Any();
                }, timeout);
            }
            catch (WebDriverTimeoutException ex)
            {
                HandleTimeoutException(ex, elementState, selector, foundElements);
            }
            return resultElements.AsReadOnly();
        }

        public void LogElementAction(string message, params object[] args)
        {
            Logger.Instance.Info(string.Concat( $"{name} :: ", string.Format(message, args)));
        }

        private void HandleElementException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (InvalidElementStateException exception)
            {
                Logger.Instance.Fatal($"{name} :: Invalid element state", exception);
                new AShot().CoordsProvider(new WebDriverCoordsProvider())
                    .ImageCropper(new IndentCropper().AddIndentFilter(new BlurFilter()))
                    .TakeScreenshot(Browser.GetInstance().GetDriver(), GetElement())
                    .getImage()
                    .Save(Path.Combine(FileProvider.GetOutputDirectory(), $"FailedElement_{DateTime.Now.ToFileTime()}.png"));
            }
        }

        private void HandleTimeoutException(WebDriverTimeoutException ex, ElementStates elementState, By selector, List<IWebElement> foundElements)
        {
            var message = $"No elements with selector '{selector}' were found in {elementState.StateName} elementState";
            if (elementState.IsCatchingTimeoutException)
            {
                if (!foundElements.Any())
                {
                    if (elementState.IsThrowingNoSuchElementException)
                    {
                        throw new NoSuchElementException(message);
                    }
                    Logger.Instance.Debug($"No elements with selector '{selector}' were found in {elementState.StateName} elementState");
                }
                else
                {
                    Logger.Instance.Debug($"Elements were found by selector '{selector}' but not in elementState {elementState.StateName}");
                }
            }
            else
            {
                var combinedMessage = $"{ex.Message}: {message}";
                if (elementState.IsThrowingNoSuchElementException && !foundElements.Any())
                {
                    throw new NoSuchElementException(combinedMessage);
                }
                throw new WebDriverTimeoutException(combinedMessage);
            }
        }

        private ElementStates ResolveState(ElementState elementState)
        {
            Func<IWebElement, bool> elementStateCondition;
            switch (elementState)
            {
                case ElementState.Displayed:
                    elementStateCondition = element => element.Displayed;
                    break;
                case ElementState.Exists:
                    elementStateCondition = element => true;
                    break;
                default:
                    throw new InvalidOperationException($"{elementState} elementState is not recognized");
            }
            return new ElementStates(elementStateCondition, elementState.ToString());
        }
    }
}

