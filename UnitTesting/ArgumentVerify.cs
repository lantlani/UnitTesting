using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Is = Rhino.Mocks.Constraints.Is;

namespace UnitTesting
{
    class ArgumentVerify
    {
        [Test]
        public void ComparePropertyExample()
        {
            MockRepository mocks = new MockRepository();
            IWebService mockService = mocks.CreateMock<IWebService>();

            using (mocks.Record())
            {
                mockService.LogError(new TraceMessage("", 0, ""));

                LastCall.Constraints(
                    Property.Value("Message", "My Message")
                    && Property.Value("Severity", 100)
                    && Property.Value("Source", "Any Source"));

            }

            mockService.LogError(new TraceMessage("My Message", 100, "Any Source"));
            mocks.VerifyAll();
        }

        [Test]
        public void CompareObjectWithDelegateExample()
        {
            MockRepository mocks = new MockRepository();
            IWebService mockService = mocks.CreateMock<IWebService>();

            using (mocks.Record())
            {
                mockService.LogError(new TraceMessage("", 0, ""));

                LastCall.Constraints(
                    Is.Matching<TraceMessage>(
                        delegate(TraceMessage msg)
                        {
                            if (msg.Severity < 50 && msg.Message.Contains("a"))
                            {
                                return false;
                            }
                            return true;
                        }
                        ));
            }

            mockService.LogError(new TraceMessage("My Message", 100, "Any Source"));
            mocks.VerifyAll();
        }

        [Test]
        public void CompareObjectWithCallbacksExample()
        {
            MockRepository mocks = new MockRepository();
            IWebService mockService = mocks.CreateMock<IWebService>();

            using (mocks.Record())
            {
                mockService.LogError(new TraceMessage("", 0, ""));
                LastCall.Constraints(Is.Matching<TraceMessage>(verifyComplexMessage));
            }

            mockService.LogError(new TraceMessage("My Message", 100, "Any Source"));
            mocks.VerifyAll();
        }

        private bool verifyComplexMessage(TraceMessage msg)
        {
            if( msg.Severity < 50 && msg.Message.Contains("a") )
                return false;

            return true;
        }
    }
}
