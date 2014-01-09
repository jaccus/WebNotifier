namespace WebNotifier.d2jsp
{
    using System.Threading;
    using System.Windows;

    internal class StatusUpdater
    {
        private Thread updaterThread;

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
            this.updaterThread = new Thread(this.UpdateStatus)
                {
                    IsBackground = true
                };

            this.updaterThread.Start();
        }
    }
}