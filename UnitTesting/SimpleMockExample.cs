using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace UnitTesting
{
   
    class SimpleMockExample
    {
        [Test]
        public void SimpleRecodingExample()
        {
            MockRepository mocks = new MockRepository();
            IWebService simulatedService = mocks.StrictMock<IWebService>();

            using (mocks.Record())
            {
                simulatedService.LogError("Too Short File Name:abc.ext");
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
