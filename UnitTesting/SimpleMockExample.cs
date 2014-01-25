using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace UnitTesting
{
    public class TraceMessage
    {
        public string Message { get; set; }
        public int Severity { get; set; }
        public string Source { get; set; }

        public TraceMessage(string _strMsg, int _iSeverity, string _strSource)
        {
            Message = _strMsg;
            Severity = _iSeverity;
            Source = _strSource;
        }
    }
    public interface IWebService
    {
        void LogError(string _strMsg);
        void LogError(TraceMessage _objTrace);
    }

    public class LogAnalyzer
    {
        private IWebService service;

        public LogAnalyzer(IWebService service)
        {
            this.service = service;
        }

        public void Analyze(string fileName)
        {
            if (fileName.Length < 8)
            {
                service.LogError("Filename too short:"+fileName);
            }
        }
    }

    public interface IGetResults
    {
        int GetSomeNumber(string _strInput);
    }
    class SimpleMockExample
    {
        [Test]
        public void SimpleRecodingExample()
        {
            MockRepository mocks = new MockRepository();
            IWebService simulatedService = mocks.StrictMock<IWebService>();

            using (mocks.Record())
            {
                simulatedService.LogError("Filename too short:abc.ext");
            }

            LogAnalyzer log = new LogAnalyzer(simulatedService);
            string tooShortFileName = "abc.ext";
            log.Analyze(tooShortFileName);

            mocks.Verify(simulatedService);

        }

        [Test]
        public void ConstraintWithStringMatchingExample()
        {
            MockRepository mocks = new MockRepository();
            IWebService mockService = mocks.CreateMock<IWebService>();

            using (mocks.Record())
            {
                mockService.LogError("Ignored String.");
                LastCall.Constraints(new Rhino.Mocks.Constraints.Contains("abc"));
            }
            
            mockService.LogError(Guid.NewGuid() + "abc");
            mocks.VerifyAll();
        }
        [Test]
        public void ReturnResultsFromStubExample()
        {
            MockRepository mocks = new MockRepository();
            IGetResults resultGetter = mocks.Stub<IGetResults>();

            using (mocks.Record())
            {
                resultGetter.GetSomeNumber("a");
                LastCall.Return(1);
            }

            int result = resultGetter.GetSomeNumber("a");
            Assert.AreEqual(1, result);

        }

    }
}
