using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Allure.Commons;
using Allure.Commons.Model;
using AutomationFramework.Browsers;
using AutomationFramework.Utilities;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Logger = AutomationFramework.Logging.Logger;

namespace OnlinerTestNUnit.Base
{
    public class BaseTest
    {
        private static AllureLifecycle Allure => AllureLifecycle.Instance;

        [OneTimeSetUp]
        public void SetupForAllure()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
            var nunitFixture = TestExecutionContext.CurrentContext.CurrentTest;

            var fixture = new TestResultContainer
            {
                uuid = nunitFixture.Id,
                name = nunitFixture.ClassName
            };
            Allure.StartTestContainer(fixture);
        }

        [OneTimeTearDown]
        public void AllureAfterAllTestsInClass()
        {
            AddMissedTests();

            var nunitFixture = TestExecutionContext.CurrentContext.CurrentTest;

            Allure.StopTestContainer(nunitFixture.Id, true);
            Allure.WriteTestContainer(nunitFixture.Id);
        }

        [SetUp]
        public void BeforeTest()
        {
            var nunitTest = TestExecutionContext.CurrentContext.CurrentTest;

            var testResult = new Allure.Commons.Model.TestResult
            {
                uuid = nunitTest.Id,
                historyId = nunitTest.FullName,
                name = nunitTest.MethodName,
                fullName = nunitTest.FullName,
                labels = new List<Label>
                {
                    Label.Suite(nunitTest.ClassName),
                    Label.Thread(),
                    Label.Host(),
                    Label.TestClass(nunitTest.ClassName),
                    Label.TestMethod(nunitTest.MethodName),
                    Label.Package(nunitTest.Fixture.ToString().Replace('+', '.'))
                }
            };

            Allure.StartTestCase(testResult);
            Browser.GetInstance().WindowMaximize();
        }

        [TearDown]
        public void AfterTest()
        {
            Browser.GetInstance().Quit();
            var nunitTest = TestExecutionContext.CurrentContext.CurrentTest;

            AttachTestLog(nunitTest.Id);
            Allure.UpdateTestCase(nunitTest.Id, x =>
            {
                x.statusDetails = new StatusDetails
                {
                    message = TestContext.CurrentContext.Result.Message,
                    trace = TestContext.CurrentContext.Result.StackTrace
                };
                x.status = GetNunitStatus(TestContext.CurrentContext.Result.Outcome);
                x.attachments.AddRange(AllureHelper.GetAttaches());
            });

            Allure.StopTestCase(nunitTest.Id);
            Allure.WriteTestCase(nunitTest.Id);
        }

        public void Step(string name) => Step(name, () => { });

        public void Step(string name, Action action)
        {
            var caseId = TestExecutionContext.CurrentContext.CurrentTest.Id;
            var uuid = Guid.NewGuid().ToString("N");
            Allure.StartStep(caseId, uuid, new StepResult()
            {
                name = name
            });

            int logLength = TestExecutionContext.CurrentContext.CurrentResult.Output.Length;
            Exception stepException = null;
            try
            {
                action();
            }
            catch (Exception e)
            {
                stepException = e;
                AllureHelper.AttachPng("FailedElement", File.ReadAllBytes(new DirectoryInfo(FileProvider.GetFailedScreensDirectory())
                    .GetFiles().OrderByDescending(file => file.LastWriteTime).First().FullName));
                AllureHelper.AttachPng("Screenshot", 
                    File.ReadAllBytes(ScreenshotProvider.PublishScreenshot($"Screenshot_{DateTime.Now.ToFileTime()}")));
                throw;
            }
            finally
            {
                Allure.UpdateStep(uuid, x =>
                {
                    x.status = GetStatusFromException(stepException);
                    x.statusDetails = new StatusDetails
                    {
                        message = stepException?.Message,
                        trace = stepException?.StackTrace
                    };
                    x.attachments.AddRange(AllureHelper.GetAttaches());
                });
                Allure.StopStep(uuid);
            }
        }

        private void AttachTestLog(string caseId)
        {
            var logAttach = GetTestLog();
            if (logAttach != null)
                Allure.UpdateTestCase(caseId, x => x.attachments.Add(logAttach));
        }

        private Attachment GetTestLog()
        {
            var attachFile = Logger.GetLogLocation();

            return new Attachment()
            {
                name = "Output",
                source = attachFile,
                type = "text/plain"
            };
        }

        private void AddMissedTests()
        {
            var nunitFixtureResult = TestExecutionContext.CurrentContext.CurrentResult;
            if (nunitFixtureResult.ResultState.Site == FailureSite.SetUp)
            {
                var failedTestResults = nunitFixtureResult.Children.Where(r => r is TestCaseResult);
                foreach (var testResult in failedTestResults)
                {
                    AddMissedTest(testResult);
                }
            }
        }

        private Status GetStatusFromException(Exception e)
        {
            if (e == null || e is SuccessException)
                return Status.passed;
            if (e is IgnoreException)
                return Status.skipped;
            if (e is InconclusiveException)
                return Status.none;
            if (e.GetType().ToString().Contains("Assert"))
                return Status.failed;

            return Status.broken;
        }

        private void AddMissedTest(ITestResult result)
        {
            var testResult = new Allure.Commons.Model.TestResult
            {
                uuid = result.Test.Id,
                historyId = result.Test.FullName,
                name = result.Test.MethodName,
                fullName = result.Test.FullName,
                labels = new List<Label>
                {
                    Label.Suite(result.Test.ClassName),
                    Label.Thread(),
                    Label.Host(),
                    Label.TestClass(result.Test.ClassName),
                    Label.TestMethod(result.Test.MethodName),
                    Label.Package(result.Test.Fixture?.ToString() ?? result.Test.ClassName)
                },
                status = GetNunitStatus(result.ResultState),
                statusDetails = new StatusDetails
                {
                    message = result.Message,
                    trace = result.StackTrace
                }
            };

            Allure.StartTestCase(testResult);
            Allure.StopTestCase(result.Test.Id);
            Allure.WriteTestCase(result.Test.Id);
        }

        private static Status GetNunitStatus(ResultState result)
        {
            switch (result.Status)
            {
                case TestStatus.Inconclusive:
                    return Status.none;
                case TestStatus.Skipped:
                    return Status.skipped;
                case TestStatus.Passed:
                    return Status.passed;
                case TestStatus.Warning:
                    return Status.broken;
                case TestStatus.Failed:
                    if (String.IsNullOrEmpty(result.Label))
                        return Status.failed;
                    else
                        return Status.broken;
                default:
                    return Status.none;
            }
        }
    }
}