namespace WebNotifier
{
    using System.Windows;
    using d2jsp;

    public partial class MainWindow
    {
        public static MainWindow Instance { get; set; }

        public string TrayIconSource { get; set; }

        private readonly StatusUpdater _statusUpdater = new StatusUpdater();

        public MainWindow()
        {
            TrayIconSource = "Icons/Loading.ico";

            InitializeComponent();
            
            Instance = this;

            WebNotifierIcon.DataContext = this;

            _statusUpdater.Start();
        }

        private void LoginActionCicked(object sender, RoutedEventArgs e)
        {
            ShowBalloonTip();
        }

        private void ExitActionClicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void HideBalloonTip()
        {
            WebNotifierIcon.HideBalloonTip();
        }

        private void ShowBalloonTip()
        {
            WebNotifierIcon.ShowBalloonTip("WebNotifier", "Balloon", WebNotifierIcon.Icon);
        }
    }
}