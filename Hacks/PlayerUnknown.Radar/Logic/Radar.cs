namespace PlayerUnknown.Radar.Logic
{
    using System;
    using System.Threading.Tasks;

    using PlayerUnknown.Radar.Enums;

    public class Radar
    {
        /// <summary>
        /// Gets the network.
        /// </summary>
        public RadarNetwork Network
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the radar mode.
        /// </summary>
        public RadarMode Mode
        {
            get
            {
                return this.Network.Mode;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Radar"/> class.
        /// </summary>
        /// <param name="Mode">The mode.</param>
        public Radar(RadarMode Mode = RadarMode.UseNetwork)
        {
            this.Network = new RadarNetwork(Mode);
        }

        /// <summary>
        /// Starts the <see cref="Radar"/>.
        /// </summary>
        public void Start()
        {
            switch (this.Mode)
            {
                case RadarMode.UseNetwork:
                {
                    this.Network.StartSniffing();
                    break;
                }

                case RadarMode.UseLogs:
                {
                    Task.Run(() => this.Network.ReadLogs());
                    break;
                }

                default:
                {
                    throw new Exception("Invalid RadarMode at Start().");
                }
            }
        }

        /// <summary>
        /// Stops the <see cref="Radar"/>.
        /// </summary>
        public void Stop()
        {
            switch (this.Mode)
            {
                case RadarMode.UseNetwork:
                {
                    this.Network.StopSniffing();
                    break;
                }

                case RadarMode.UseLogs:
                {
                    break;
                }

                default:
                {
                    throw new Exception("Invalid RadarMode at Stop().");
                }
            }
        }
    }
}
