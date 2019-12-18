using System;
using System.IO;
using Allure.Commons;
using NLog;
using NUnit.Framework;

namespace AutomationFramework
{
    
    public class BaseSteps : AllureReport
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        [OneTimeSetUp]
        public void SetupForAllure()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
        }
    }
}