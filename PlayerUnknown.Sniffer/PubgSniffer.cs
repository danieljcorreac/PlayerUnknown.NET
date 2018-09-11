namespace PlayerUnknown.Sniffer
{
    using System;
    using System.IO;

    using PacketDotNet;

    using SharpPcap;
    using SharpPcap.WinPcap;

    public class PubgSniffer
    {
        /// <summary>
        /// Gets the device used to capture packets.
        /// </summary>
        public WinPcapDevice Device
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets or sets the OnPacketCaptured event handler.
        /// </summary>
        public EventHandler<PubgPacket> OnPacketCaptured
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is capturing packets.
        /// </summary>
        public bool IsCapturing
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        public bool IsConfigured
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is saving the packets.
        /// </summary>
        public bool IsSaving
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> at which this instance started sniffing packet.
        /// </summary>
        public DateTime StartTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public PubgSniffer(bool SavePackets = false)
        {
            this.IsSaving = SavePackets;
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        public bool TryConfigure()
        {
            if (this.IsConfigured)
            {
                return false;
            }

            var Devices = CaptureDeviceList.Instance;

            if (Devices.Count == 0)
            {
                return false;
            }

            Logging.Info(typeof(PubgSniffer), "Detected " + Devices.Count + " network cards.");

            foreach (var Device in Devices)
            {
                Logging.Info(typeof(PubgSniffer), Device.GetType().Name + ", " + Device.Description +  ".");
            }

            this.Device = (WinPcapDevice) Devices[4];

            if (Device.Started == false)
            {
                this.Device.Open(OpenFlags.DataTransferUdp, 1000);
                this.Device.Filter = "udp src portrange 7000-8999 or udp[4:2] = 52";

                this.Device.OnPacketArrival     += this.OnPacketSniffed;
                this.Device.OnCaptureStopped    += this.OnCaptureStopped;

                this.IsConfigured               = true;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Starts capturing packets.
        /// </summary>
        public void StartCapture()
        {
            if (this.Device == null)
            {
                throw new Exception("Device was null at StartCapture().");
            }

            if (this.IsCapturing == true)
            {
                throw new Exception("StartCapture() called twice.");
            }

            this.IsCapturing = true;
            this.StartTime   = DateTime.Now;

            this.Device.StartCapture();
        }

        /// <summary>
        /// Stops capturing packets.
        /// </summary>
        public void StopCapture()
        {
            if (this.Device == null)
            {
                return;
            }

            if (this.IsCapturing == false)
            {
                return;
            }

            this.IsCapturing = false;

            this.Device.StopCapture();
        }

        /// <summary>
        /// Called when a packet has been sniffed.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The <see cref="CaptureEventArgs"/> instance containing the event data.</param>
        private void OnPacketSniffed(object Sender, CaptureEventArgs Args)
        {
            var UdpPacket = Packet.ParsePacket(Args.Packet.LinkLayerType, Args.Packet.Data);

            if (UdpPacket == null)
            {
                return;
            }

            // We want to be sure the packet is a packet we actually want

            Logging.Warning(typeof(PubgSniffer), "LinkLayerType == " + Args.Packet.LinkLayerType.ToString());

            if (Args.Packet.LinkLayerType == LinkLayers.Null)
            {
                return; // LinkLayers.Null is not implemented !
            }

            // We want the payload, so we take the last part of the packet

            while (UdpPacket.PayloadPacket != null)
            {
                UdpPacket = UdpPacket.PayloadPacket;
            }

            // We call the event handlers if there is one

            if (this.OnPacketCaptured != null)
            {
                this.OnPacketCaptured.Invoke(null, PubgPacket.FromBuffer(UdpPacket.PayloadData));
            }

            if (this.IsSaving)
            {
                File.AppendAllText("Logs\\Packets." + this.StartTime.ToString("MM-dd-yyyy.hh-mm-ss") + ".log", BitConverter.ToString(UdpPacket.PayloadData) + Environment.NewLine);
            }
        }

        /// <summary>
        /// Called when the capturing has stopped.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The arguments.</param>
        private void OnCaptureStopped(object Sender, CaptureStoppedEventStatus Args)
        {
            Logging.Warning(this.GetType(), "Packet capture has been stopped.");
        }
    }
}
