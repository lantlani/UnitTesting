using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                service.LogError("Too Short File Name:" + fileName);
            }
        }
    }

    public interface IGetResults
    {
        int GetSomeNumber(string _strInput);
    }
}
