namespace WebNotifier
{
    using System.Windows;
    using d2jsp;

    public partial class MainWindow
    {
        private readonly StatusUpdater statusUpdater = new StatusUpdater();

        public MainWindow()
        {
            this.TrayIconSource = "Icons/Loading.ico";

            InitializeComponent();
            
            Instance = this;

            WebNotifierIcon.DataContext = this;

            this.statusUpdater.Start();
        }

        public static MainWindow Instance { get; set; }

        public string TrayIconSource { get; set; }

        private void LoginActionCicked(object sender, RoutedEventArgs e)
        {
            this.ShowBalloonTip();
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