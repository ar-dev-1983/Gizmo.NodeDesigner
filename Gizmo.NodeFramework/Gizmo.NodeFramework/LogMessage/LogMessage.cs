using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gizmo.NodeFramework
{
    public class LogMessage
    {
        public string Message { set; get; }
        public LogMessageType MessageType { set; get; }
        public DateTime TimeStamp { private set; get; }

        public LogMessage()
        {
            Message = string.Empty;
            MessageType =  LogMessageType.Information;
            TimeStamp = DateTime.Now;
        }

        public LogMessage(string message, LogMessageType type)
        {
            Message = message;
            MessageType = type;
            TimeStamp = DateTime.Now;
        }

    }
}
