namespace PlayerUnknown.Radar.Logic
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Ayx.BitIO;

    using PlayerUnknown.Native.Structures;
    using PlayerUnknown.Radar.Enums;
    using PlayerUnknown.Sniffer;

    public class RadarNetwork
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
        /// Gets the mode.
        /// </summary>
        public RadarMode Mode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is capturing packets.
        /// </summary>
        public bool IsCapturing
        {
            get
            {
                if (this.Sniffer == null)
                {
                    return false;
                }

                return this.Sniffer.IsCapturing;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadarNetwork"/> class.
        /// </summary>
        /// <param name="Mode">The mode.</param>
        public RadarNetwork(RadarMode Mode = RadarMode.UseNetwork)
        {
            this.Sniffer = new PubgSniffer();

            if (Mode == RadarMode.UseNetwork)
            {
                this.Sniffer.OnPacketCaptured += OnPacketCaptured;
                this.Sniffer.TryConfigure();
            }
        }

        /// <summary>
        /// Reads the logs.
        /// </summary>
        public void ReadLogs()
        {
            var LogsDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Logs"));

            if (LogsDir.Exists == false)
            {
                LogsDir.Create();
            }

            var Log = LogsDir.GetFiles("*.log").OrderByDescending(T => T.LastWriteTime).FirstOrDefault();

            if (Log != null)
            {
                Logging.Warning(typeof(RadarNetwork), "Reading log file " + Log.Name + ".");

                foreach (var Packet in File.ReadAllLines(Log.FullName))
                {
                    this.OnPacketCaptured(null, PubgPacket.FromHexa(Packet));
                }
            }
            else
            {
                Logging.Warning(typeof(RadarNetwork), "RadarMode == UseLogs but no logs found.");
            }
        }

        /// <summary>
        /// Starts to capture UDP packets.
        /// </summary>
        public void StartSniffing()
        {
            if (this.Mode != RadarMode.UseNetwork)
            {
                throw new Exception("Can't start sniffing if SniffingMode == UseNetwork.");
            }

            this.Sniffer.StartCapture();
        }

        /// <summary>
        /// Starts to capture UDP packets.
        /// </summary>
        public void StopSniffing()
        {
            if (this.Mode != RadarMode.UseNetwork)
            {
                throw new Exception("Can't stop sniffing if SniffingMode == UseNetwork.");
            }

            this.Sniffer.StopCapture();
        }

        /// <summary>
        /// Called when a packet has been captured.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="PubgPacket">The pubg packet.</param>
        private void OnPacketCaptured(object Sender, PubgPacket PubgPacket)
        {
            byte[] Buffer = PubgPacket.Buffer;

            BitReader Reader = new BitReader(Buffer);
            {
                // Read bytes..

                bool IsHandshake    = Reader.ReadBool();
                bool IsEncrypted    = Reader.ReadBool();

                int PacketId        = Reader.ReadInt(8);

                // Logging.Info(typeof(RadarNetwork), "[" + PacketId + "] Packet is " + (IsHandshake ? (IsEncrypted ? "an encrypted handshake" : "an unencrypted handshake") : (IsEncrypted ? "a simple encrypted" : "a simple unencrypted")) + " packet.");
            }

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
                    float Y = Buffer[Buffer.Length - 8  + 1] << 16 | Buffer[Buffer.Length - 8  + 2] << 8 | Buffer[Buffer.Length - 8  + 3];
                    float Z = Buffer[Buffer.Length - 4  + 1] << 16 | Buffer[Buffer.Length - 4  + 2] << 8 | Buffer[Buffer.Length - 4  + 3];

                    // Logging.Info(typeof(RadarNetwork), "BasePosition : " + ("[ X : " + X + ", Y : " + Y + ", Z : " + Z + "]"));
                    
                    X = 0.1250155302572263f     * X - 20.58662848625851f;
                    Y = -0.12499267869373985f   * Y + 2097021.7946571815f;
                    Z = Z / 20.0f;

                    var SelfPosition = Position.New(X, Y, Z);

                    Logging.Info(typeof(RadarNetwork), "SelfPosition : " + SelfPosition);
                }
            }
            else
            {
                if (true)
                {
                    if (Encoding.UTF8.GetString(Buffer).Contains("Miramar"))
                    {
                        Logging.Warning(typeof(RadarNetwork), "Miramar !");
                    }
                    else if (Encoding.UTF8.GetString(Buffer).Contains("Erangel"))
                    {
                        Logging.Warning(typeof(RadarNetwork), "Erangel !");
                    }
                }
            }
        }
    }
}