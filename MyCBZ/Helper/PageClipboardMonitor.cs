using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Win_CBZ.Helper
{
    internal class PageClipboardMonitor
    {
        
        Thread MonitorThread;

        public event EventHandler<ClipboardChangedEvent> ClipboardChanged;

        bool IsStopped = false;

        public PageClipboardMonitor() 
        {
            MonitorThread = new Thread(MonitorProc);
            MonitorThread.Start();
        }


        public void StopMonitoring()
        {
            IsStopped = true;

            if (MonitorThread != null)
            {
                if (MonitorThread.IsAlive)
                {
                    MonitorThread.Abort();
                }
            }
        }


        
        protected void MonitorProc(object threadParams = null)
        {
            object clipObject = null;

            while (!IsStopped)
            {
                clipObject = Clipboard.GetDataObject();

                Thread.Sleep(1000);
            }
        }

        protected virtual void OnClipboardChanged(ClipboardChangedEvent e)
        {
            ClipboardChanged?.Invoke(this, e);
        }
    }
}
