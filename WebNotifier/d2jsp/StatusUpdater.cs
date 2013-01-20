using System.Windows;

namespace WebNotifier.d2jsp
{
    using System.Threading;

    class StatusUpdater
    {
        private Thread _thread;

        public void UpdateStatus()
        {
            try
            {
                var unreadCount = new D2JspProcessor().ReadInboxUnreadCount();

                MainWindow.Instance.TrayIconSource = unreadCount == 0 ? "/Icons/Ok.ico" : "/Icons/Msg.ico";
            }
            catch (WebNotifierException e)
            {
                MessageBox.Show(string.Format("Failed to update WebNotifier status:\n{0}", e.Message));
            }
        }

        public void Start()
        {
            _thread = new Thread(UpdateStatus)
                {
                    IsBackground = true
                };

            _thread.Start();
        }
    }
}