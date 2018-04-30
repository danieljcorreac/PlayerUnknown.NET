namespace PlayerUnknown.Radar.Logic
{
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
        /// Initializes a new instance of the <see cref="Radar"/> class.
        /// </summary>
        public Radar()
        {
            this.Network = new RadarNetwork();
        }
    }
}
