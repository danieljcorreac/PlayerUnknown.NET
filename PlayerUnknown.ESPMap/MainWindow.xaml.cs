namespace PlayerUnknown.ESPMap
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Gets the <see cref="PubgEsp"/>.
        /// </summary>
        public PubgEsp ESP
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="MainWindow"/> is initialized.
        /// </summary>
        public bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="MainWindow"/> is rendering the map.
        /// </summary>
        public bool IsRendering
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
        /// Démarre le processus d’initialisation pour cet élément.
        /// </summary>
        public override void BeginInit()
        {
            if (this.Initialized)
            {
                return;
            }

            if (this._contentLoaded)
            {
                if (PUBG.Initialized)
                {
                    return;
                }

                PUBG.Initialize();
                PUBG.Attach();
                PUBG.EnableEvents();
            }
            else
            {
                Logging.Warning(this.GetType(), "Content not loaded at BeginInit().");
            }
        }

        /// <summary>
        /// Indique que le processus d’initialisation pour l’élément est terminé.
        /// </summary>
        public override void EndInit()
        {
            if (this.Initialized)
            {
                return;
            }

            if (this._contentLoaded)
            {
                PubgMap.Initialize();

                if (PUBG.IsAttached)
                {
                    this.ESP = new PubgEsp(this);

                    if (this.ESP.TryConfigure())
                    {
                        this.ESP.Start();
                    }
                    else
                    {
                        Logging.Info(this.GetType(), "TryConfigure() != true at EndInit().");
                    }

                    this.Initialized = true;

                    if (this.IsRendering == false)
                    {
                        Logging.Info(this.GetType(), "Rendering().");

                        this.Render().ConfigureAwait(false);
                    }
                }
                else
                {
                    Logging.Info(this.GetType(), "PUBG.IsAttached() != true at EndInit().");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "Content not loaded at EndInit().");
            }
        }

        /// <summary>
        /// Renders the map.
        /// </summary>
        private async Task Render()
        {
            this.IsRendering = true;

            while (true)
            {
                if (this.ESP.Battle.Started)
                {
                    Bitmap MapImage = this.ESP.Battle.PlayingMap;

                    if (MapImage != null)
                    {
                        // TODO: Draw.

                        this.SetImage(MapImage);
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "MapImage == null at Render().");
                    }
                }
                else
                {
                    this.SetImage(PubgMap.GetMap("Erangel"));
                }

                await Task.Delay(250).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="Image">The image.</param>
        public void SetImage(Bitmap Image)
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                if (Image == null)
                {
                    this.MapImage.Source = null;
                }
                else
                {
                    using (var Stream = new MemoryStream())
                    {
                        Image.Save(Stream, ImageFormat.Png);
                        Stream.Position = 0;

                        var BitmapImage = new BitmapImage();
                        BitmapImage.BeginInit();
                        BitmapImage.StreamSource = Stream;
                        BitmapImage.CacheOption = BitmapCacheOption.None;
                        BitmapImage.EndInit();

                        this.MapImage.Source = BitmapImage;
                    }
                }
            });
        }

        /// <summary>
        /// Executes an action when the left button of the mouse is pushed.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void WindowMouseLeftButtonDown(object Sender, MouseButtonEventArgs Args)
        {
            DragMove();
        }
    }
}
