namespace WebNotifier
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using Hardcodet.Wpf.TaskbarNotification;

    /// <summary>
    /// Interaction logic for FancyBalloon.xaml
    /// </summary>
    public partial class CuteBalloon
    {
        /// <summary>
        /// Description
        /// </summary>
        public static readonly DependencyProperty BalloonTextProperty = DependencyProperty.Register(
            "BalloonText",
            typeof(string),
            typeof(CuteBalloon),
            new FrameworkPropertyMetadata(string.Empty));

        private bool isClosing;

        public CuteBalloon()
        {
            InitializeComponent();
            TaskbarIcon.AddBalloonClosingHandler(this, this.OnBalloonClosing);
        }

        /// <summary>
        /// A property wrapper for the <see cref="BalloonTextProperty"/>
        /// dependency property:<br/>
        /// Description
        /// </summary>
        public string BalloonText
        {
            get { return (string)GetValue(BalloonTextProperty); }
            set { SetValue(BalloonTextProperty, value); }
        }

        /// <summary>
        /// By subscribing to the <see cref="TaskbarIcon.BalloonClosingEvent"/>
        /// and setting the "Handled" property to true, we suppress the popup
        /// from being closed in order to display the fade-out animation.
        /// </summary>
        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.isClosing = true;
        }

        /// <summary>
        /// Resolves the <see cref="TaskbarIcon"/> that displayed
        /// the balloon and requests a close action.
        /// </summary>
        private void ImgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // the tray icon assigned this attached property to simplify access
            var taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
        }

        /// <summary>
        /// If the users hovers over the balloon, we don't close it.
        /// </summary>
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            // if we're already running the fade-out animation, do not interrupt anymore (makes things too complicated for the sample)
            if (this.isClosing)
            {
                return;
            }

            // the tray icon assigned this attached property to simplify access
            var taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.ResetBalloonCloseTimer();
        }

        /// <summary>
        /// Closes the popup once the fade-out animation completed.
        /// The animation was triggered in XAML through the attached
        /// BalloonClosing event.
        /// </summary>
        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            var pp = (Popup)Parent;
            pp.IsOpen = false;
        }
    }
}