namespace PlayerUnknown.Test
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using PlayerUnknown.Cheats;
    using PlayerUnknown.Events;
    using PlayerUnknown.Leaker;
    using PlayerUnknown.Logic.Weapons;
    using PlayerUnknown.Native;
    using PlayerUnknown.Sniffer;

    using WinApi.Gdi32;
    using WinApi.User32;
    using WinApi.Windows;

    using Window = WinApi.Windows.Controls.Window;

    internal class Program
    {
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

                if (PUBG.IsAttached == false)
                {
                    Logging.Info(typeof(Program), "Waiting for PUBG to start...");

                    while (PUBG.IsAttached != true)
                    {
                        Thread.Sleep(500);
                    }

                    Thread.Sleep(2500);
                }

                // Program.TestLeaker();
                // Program.TestSniffer();
                // Program.TestNoRecoil();
                // Program.TestFastFire();
                // Program.TestWindow();
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

                // Setup events.

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

        /// <summary>
        /// Tests the leaker.
        /// </summary>
        internal static void TestLeaker()
        {
            FuckBattlEye.Run("PlayerUnknown.Test.exe", "TslGame");
        }

        /// <summary>
        /// Tests the window.
        /// </summary>
        internal static void TestWindow()
        {
            // Window.SetTitle(PUBG.WindowHandle, "PLAYERUNKNOWN'S HACKEDGROUNDS");

            using (var Overlay = Window.Create<TestWindow>("PubgOverlay", exStyles: WindowExStyles.WS_EX_TOPMOST | WindowExStyles.WS_EX_TRANSPARENT))
            {
                Overlay.Show();

                Overlay.SetPosition(PUBG.Window.NormalPosition);
                Overlay.SetSize(PUBG.Window.NormalPosition.Width, PUBG.Window.NormalPosition.Height);

                int ExitCode = new EventLoop().Run(Overlay);
            }
        }
    }
}
