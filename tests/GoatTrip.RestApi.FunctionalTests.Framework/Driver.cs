using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace GoatTrip.RestApi.FunctionalTests.Framework
{
    public class Driver
    {
        public static IWebDriver Instance { get; private set; }

        public static void Initialise()
        {
            NewInstance("default");
            SwitchToInstance("default");
            Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            _defaultWindowHandle = Instance.WindowHandles.ToList().Last();
        }

        public static void Close()
        {
            foreach (var key in _drivers.Keys.ToList())
            {
                _drivers[key].Close();
                _drivers.Remove(key);
            }
        }

        public static string NewWindow()
        {
            var jscript = Instance as IJavaScriptExecutor;
            jscript.ExecuteScript("window.open()");
            return Instance.WindowHandles.ToList().Last();
        }

        public static void SwitchToWindow()
        {
            SwitchToWindow(_defaultWindowHandle);
        }

        public static void SwitchToWindow(string windowName)
        {
            Instance.SwitchTo().Window(windowName);
        }

        public static void NewInstance(string key)
        {
            _drivers.Add(key, new FirefoxDriver());
        }

        public static void SwitchToInstance()
        {
            SwitchToInstance("default");
        }

        public static void SwitchToInstance(string instanceKey)
        {
            Instance = _drivers[instanceKey];
        }

        public static TResult Wait<TResult>(double timeout, Func<IWebDriver, TResult> condition)
        {
            var wait = new WebDriverWait(Instance, TimeSpan.FromSeconds(timeout));
            return wait.Until(condition);
        }

        public static IWebElement WaitForElement(double timeout, string elementId)
        {
            return WaitForElement(timeout, By.Id(elementId));
        }

        public static IWebElement WaitForElement(double timeout, By by)
        {
            return Wait(timeout, ExpectedConditions.ElementExists(by));
        }

        public static void FindAndClickElement(By by)
        {
            var element = Instance.FindElement(by);
            element.Click();
        }

        public static void FindElementAndSendKeys(By by, string text)
        {
            var titleTextbox = Instance.FindElement(by);
            titleTextbox.SendKeys(text);
        }

        private static string _defaultWindowHandle = null;
        private static readonly Dictionary<string, IWebDriver> _drivers = new Dictionary<string, IWebDriver>();
    }
}