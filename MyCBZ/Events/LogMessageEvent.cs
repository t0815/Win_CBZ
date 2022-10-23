using CBZMage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class LogMessageEvent
    {

        public const int LOGMESSAGE_TYPE_ERROR = 1;
        public const int LOGMESSAGE_TYPE_WARNING = 2;
        public const int LOGMESSAGE_TYPE_INFO = 3;


        public int Type { get; set; }

        public String Message { get; set; }

        public DateTimeOffset MessageTime { get; set; }


        public LogMessageEvent(int type, String message)
        {
            this.Type = type;
            this.Message = message; 
            this.MessageTime = DateTimeOffset.Now;
        }   
    }
}
