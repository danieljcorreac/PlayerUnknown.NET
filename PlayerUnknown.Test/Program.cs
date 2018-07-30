namespace PlayerUnknown.Test
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using PlayerUnknown.Cheats;
    using PlayerUnknown.Logic.Weapons;
    using PlayerUnknown.Sniffer;

    public static class Program
    {
        /// <summary>
        /// Gets a value indicating whether this instance is checking.
        /// </summary>
        public static bool IsChecking
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        [STAThread]
        internal static void Main(string[] Args)
        {
            if (Args.Length > 0)
            {
                Console.WriteLine("[*] Args : " + Args[0] + ".");
            }
            else
            {
                PUBG.Initialize();
                PUBG.Attach();
                PUBG.EnableEvents();

                if (IsChecking && PUBG.IsAttached == false)
                {
                    Logging.Info(typeof(Program), "Waiting for PUBG to start...");

                    while (PUBG.IsAttached != true)
                    {
                        Thread.Sleep(500);
                    }

                    Thread.Sleep(2500);
                }

                // Program.TestSniffer();
                Program.TestNoRecoil();
            }

            Console.ReadKey(false);
        }

        /// <summary>
        /// Tests the sniffer.
        /// </summary>
        internal static void TestSniffer()
        {
            var PubgSniffer = new PubgSniffer();

            if (PubgSniffer.TryConfigure())
            {
                PubgSniffer.StartCapture();

                // Setup events..

                PubgSniffer.OnPacketCaptured += (Sender, Packet) =>
                {
                    Console.WriteLine("[*] OnPacketCaptured().");
                };
            }
            else
            {
                Logging.Info(typeof(Program), "TryConfigure() != true at TestSniffer().");
            }
        }

        /// <summary>
        /// Tests the no recoil.
        /// </summary>
        internal static void TestNoRecoil()
        {
            NoRecoil.Weapon = new Ak47();
            NoRecoil.Run().Wait();
        }

        /// <summary>
        /// Tests the fast fire.
        /// </summary>
        internal static void TestFastFire()
        {
            FastFire.Run().Wait();
        }
    }
}
