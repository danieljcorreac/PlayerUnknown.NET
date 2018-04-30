namespace PlayerUnknown.Radar.Logic
{
    using System.Text;

    using PlayerUnknown.Logic;
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
        /// Initializes a new instance of the <see cref="RadarNetwork"/> class.
        /// </summary>
        public RadarNetwork()
        {
            this.Sniffer = new PubgSniffer();
            this.Sniffer.TryConfigure();
            this.Sniffer.OnPacketCaptured += OnPacketCaptured;
            this.Sniffer.StartCapture();
        }

        /// <summary>
        /// Called when a packet has been captured.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="PubgPacket">The pubg packet.</param>
        private void OnPacketCaptured(object Sender, PubgPacket PubgPacket)
        {
            // Logging.Info(this.GetType(), "byte[" + PubgPacket.Length + "] | Captured a packet.");

            byte[] Buffer = PubgPacket.Buffer;

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
                    float Y = Buffer[Buffer.Length - 8  + 1] << 16 | Buffer[Buffer.Length - 8  + 2] << 8 | Buffer[Buffer.Length - 8  + 3];
                    float Z = Buffer[Buffer.Length - 4  + 1] << 16 | Buffer[Buffer.Length - 4  + 2] << 8 | Buffer[Buffer.Length - 4  + 3];

                    X = 0.1250155302572263f     * X - 20.58662848625851f;
                    Y = -0.12499267869373985f   * Y + 2097021.7946571815f;
                    Z = Z / 20.0f;

                    var SelfPosition = Position.New(X, Y, Z);

                    Logging.Warning(this.GetType(), "SelfPosition : " + SelfPosition);
                }
            }
            else
            {
                if (true)
                {
                    if (Encoding.UTF8.GetString(Buffer).Contains("Miramar"))
                    {
                        Logging.Info(this.GetType(), "Miramar !");
                    }
                    else if (Encoding.UTF8.GetString(Buffer).Contains("Erangel"))
                    {
                        Logging.Info(this.GetType(), "Erangel !");
                    }
                }
            }
        }
    }
}