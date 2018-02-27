namespace PlayerUnknown.Sniffer
{
    using System;
    using System.Threading;

    using SharpPcap;
    using SharpPcap.WinPcap;

    public class PubgSniffer
    {
        /// <summary>
        /// Gets the device used to sniff packets.
        /// </summary>
        private WinPcapDevice Device
        {
            get;
            set;
        }

        private long PacketCaptured;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public PubgSniffer()
        {
            // PubgSniffer.
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        public bool TryConfigure()
        {
            var Devices = CaptureDeviceList.Instance;

            if (Devices.Count < 1)
            {
                return false;
            }

            this.Device = (WinPcapDevice) Devices[1];

            if (Device.Started == false)
            {
                this.Device.Open(OpenFlags.DataTransferUdp, 1000);
                this.Device.Filter = "udp src portrange 7000-8999 or udp[4:2] = 52";

                this.Device.OnPacketArrival     += this.OnPacketSniffed;
                this.Device.OnCaptureStopped    += this.OnCaptureStopped;

                if (this.Device.Opened)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Starts to capture packets.
        /// </summary>
        public void StartCapture()
        {
            if (this.Device == null)
            {
                return;
            }

            this.Device.StartCapture();
        }

        /// <summary>
        /// Stops the capture.
        /// </summary>
        public void StopCapture()
        {
            if (this.Device == null)
            {
                return;
            }

            this.Device.StopCapture();
        }

        /// <summary>
        /// Called when a packet has been sniffed.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The <see cref="CaptureEventArgs"/> instance containing the event data.</param>
        private void OnPacketSniffed(object Sender, CaptureEventArgs Args)
        {
            if (BitConverter.ToString(Args.Packet.Data).StartsWith("00-24-D4-B0-6B-10-6C-FD-B9-94-EF-E4-08"))
            {
                return;
            }

            if (Interlocked.Increment(ref this.PacketCaptured) > 10) // Log Limit
            {
                return;
            }

            Logging.Info(this.GetType(), "[" + Args.Packet.Data.Length + "]" + " " + BitConverter.ToString(Args.Packet.Data));
        }

        /// <summary>
        /// Called when the capturing has stopped.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The arguments.</param>
        private void OnCaptureStopped(object Sender, CaptureStoppedEventStatus Args)
        {
            Logging.Info(this.GetType(), "Packet capture has been stopped.");
        }
    }
}
