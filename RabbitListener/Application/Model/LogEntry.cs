using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RabbitListener.Application.Model
{
    public class LogEntry
    {
        public string ServiceName { get; set; }
        public string Url { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public LogEntry()
        {
            ServiceName = GetCallingClassName();
        }

        private string GetCallingClassName()
        {
            var callingType = new StackTrace().GetFrame(2).GetMethod().DeclaringType;
            return callingType != null ? callingType.Name : "Unknown";
        }
    }

}
