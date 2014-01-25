using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace UnitTesting
{
    /// <summary>
    /// These test shows same test in different manner.
    /// </summary>
    class UnitTestSyntax
    {
        /// <summary>
        /// Record-and-Replay
        /// </summary>
        [Test]
        public void CreateMock_WithReplayAll()
        {
            MockRepository mockEngine = new MockRepository();
            IWebService simulatedService = mockEngine.DynamicMock<IWebService>();

            using (mockEngine.Record())
            {
                simulatedService.LogError("Too Short File Name:abc.txt");
            }
            
            LogAnalyzer log = new LogAnalyzer(simulatedService);            
            log.Analyze("abc.txt");

            mockEngine.Verify(simulatedService);

        }

        /// <summary>
        /// Arrange-Act-Assert
        /// </summary>
        [Test]
        public void CreateMock_WithReplayAll_AAA()
        {
            MockRepository mockEngine = new MockRepository();
            IWebService simulatedService = mockEngine.DynamicMock<IWebService>();

            LogAnalyzer log = new LogAnalyzer(simulatedService);
            
            // Start Act
            mockEngine.ReplayAll();
            string tooShortFileName = "abc.txt";
            log.Analyze(tooShortFileName);

            // Assert
            simulatedService.AssertWasCalled(svc => svc.LogError("Too Short File Name:abc.txt"));
        }
    }
}
