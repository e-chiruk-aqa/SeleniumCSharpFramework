using System;
using System.IO;
using Allure.Commons;
using AutomationFramework.Logging;
using NUnit.Framework;

namespace AutomationFramework
{
    public class BaseSteps : AllureReport
    {
        public static Logger Logger = Logger.Instance;

        [OneTimeSetUp]
        public void SetupForAllure()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
        }
    }
}