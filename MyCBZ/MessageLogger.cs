using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class MessageLogger
    {

        private static MessageLogger MessageLoggerInstance = null;
        private static readonly object InstanceLock = new object();

        private event EventHandler<LogMessageEvent> LoggerEvent;

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

        public void SetHandler(EventHandler<LogMessageEvent> handler)
        {
            LoggerEvent += handler;

        }

        protected virtual void OnMessageLogged(LogMessageEvent e)
        {
            EventHandler<LogMessageEvent> handler = LoggerEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        public void Log(int type, string message)
        {
            OnMessageLogged(new LogMessageEvent(type, message));
        }
    }
}
