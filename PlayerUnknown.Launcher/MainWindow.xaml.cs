namespace PlayerUnknown.Launcher
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Gets a value indicating whether this instance if PUBG is launching.
        /// </summary>
        public bool IsPubgLaunching
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the security mode is selected.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void OnSecurityMode(object Sender, RoutedEventArgs Args)
        {
            if (this.IsPubgLaunching)
            {
                return;
            }

            this.IsPubgLaunching = true;

            var Steam = Process.Start("steam://run/578080");

            if (Steam == null)
            {
                MessageBox.Show("Neither Steam and PUBG were detected.", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.Hide();

            await Task.Delay(2500);

            if (Process.GetProcessesByName("TslGame").Length == 0)
            {
                await Task.Delay(2500);
            }

            if (Process.GetProcessesByName("TslGame").Length > 0)
            {
                while (Process.GetProcessesByName("TslGame").Length > 0)
                {
                    await Task.Delay(2500);
                }
            }

            this.IsPubgLaunching = false;

            this.Show();
        }
    }
}
