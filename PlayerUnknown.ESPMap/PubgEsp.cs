namespace PlayerUnknown.ESPMap
{
    using System.Drawing;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using PlayerUnknown.Logic;
    using PlayerUnknown.Sniffer;

    using Brushes = System.Windows.Media.Brushes;

    public class PubgEsp
    {
        /// <summary>
        /// Gets the packet sniffer.
        /// </summary>
        public PubgSniffer Sniffer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the battle handler.
        /// </summary>
        public PubgBattle Battle
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the window.
        /// </summary>
        public MainWindow Window
        {
            get;
        }

        public Position SelfPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgEsp"/> class.
        /// </summary>
        public PubgEsp()
        {
            this.Sniffer = new PubgSniffer();
            this.Battle  = new PubgBattle();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgEsp"/> class.
        /// </summary>
        /// <param name="Window">The window.</param>
        public PubgEsp(MainWindow Window) : this()
        {
            this.Window = Window;
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        public bool TryConfigure()
        {
            if (!this.Sniffer.TryConfigure())
            {
                return false;
            }

            this.Sniffer.OnPacketCaptured += this.OnPacketCaptured;

            return true;
        }

        /// <summary>
        /// Starts this instance and sniff packets.
        /// </summary>
        public void Start()
        {
            if (this.Sniffer == null)
            {
                return;
            }

            this.Sniffer.StartCapture();
        }

        /// <summary>
        /// Called when a packet has been captured.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Packet">The packet.</param>
        private void OnPacketCaptured(object Sender, PubgPacket Packet)
        {
            byte[] Buffer = Packet.Buffer;

            /* BitReader Reader = new BitReader(UdpBody.PayloadData);
            {
                // Read bytes..

                bool IsHandshake    = Reader.ReadBool();
                bool IsEncrypted    = Reader.ReadBool();

                int PacketId        = Reader.ReadInt(8);

                Logging.Info(this.GetType(), "[" + PacketId + "] Packet is " + (IsHandshake ? (IsEncrypted ? "an encrypted handshake" : "an unencrypted handshake") : (IsEncrypted ? "a simple encrypted" : "a simple unencrypted")) + " packet.");
            } */

            if (Buffer.Length == 44)
            {
                bool IsValid = true;

                if (!(Buffer[Buffer.Length - 12] == 0 || Buffer[Buffer.Length - 12] == 0xFF))
                {
                    IsValid = false;
                }

                if (!(Buffer[Buffer.Length - 8] == 0 || Buffer[Buffer.Length - 8] == 0xFF))
                {
                    IsValid = false;
                }

                if (!(Buffer[Buffer.Length - 4] == 0 || Buffer[Buffer.Length - 4] == 0xFF))
                {
                    IsValid = false;
                }

                if (IsValid)
                {
                    float X = Buffer[Buffer.Length - 12 + 1] << 16 | Buffer[Buffer.Length - 12 + 2] << 8 | Buffer[Buffer.Length - 12 + 3];
                    float Y = Buffer[Buffer.Length - 8 + 1] << 16 | Buffer[Buffer.Length - 8 + 2] << 8 | Buffer[Buffer.Length - 8 + 3];
                    float Z = Buffer[Buffer.Length - 4 + 1] << 16 | Buffer[Buffer.Length - 4 + 2] << 8 | Buffer[Buffer.Length - 4 + 3];

                    X = 0.1250155302572263f * X - 20.58662848625851f;
                    Y = -0.12499267869373985f * Y + 2097021.7946571815f;
                    Z = Z / 20.0f;

                    this.SelfPosition = Position.New(X, Y, Z);

                    if (this.Battle.Started)
                    {
                        using (Graphics Graphic = Graphics.FromImage(this.Battle.PlayingMap))
                        {
                            Graphic.DrawEllipse(Pens.Red, X / this.Battle.PlayingMap.Width, Y / this.Battle.PlayingMap.Height, 2f, 2f);
                        }

                        this.Window.SetImage(this.Battle.PlayingMap);
                    }
                }
            }
            else
            {
                if (this.Battle.Started == false)
                {
                    if (Encoding.UTF8.GetString(Buffer).Contains("Miramar"))
                    {
                        this.Battle.BeginGame("Miramar");
                        this.Window.SetImage(this.Battle.PlayingMap);
                    }
                    else if (Encoding.UTF8.GetString(Buffer).Contains("Erangel"))
                    {
                        this.Battle.BeginGame("Erangel");
                        this.Window.SetImage(this.Battle.PlayingMap);
                    }
                }
            }
        }
    }
}
