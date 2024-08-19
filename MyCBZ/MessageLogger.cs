using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Handler;

namespace Win_CBZ
{
    internal class MessageLogger
    {

        private static MessageLogger MessageLoggerInstance = null;
        private static readonly object InstanceLock = new object();

        private MessageLogger() { }


        public static MessageLogger Instance
        {
            get
            {
                lock (InstanceLock)
                {
                    if (MessageLoggerInstance == null)
                    {
                        MessageLoggerInstance = new MessageLogger();
                    }

                    return MessageLoggerInstance;
                }
            }
        }

        public void Log(int type, string message)
        {
            AppEventHandler.OnMessageLogged(this, new LogMessageEvent(type, message));
        }
    }
}
