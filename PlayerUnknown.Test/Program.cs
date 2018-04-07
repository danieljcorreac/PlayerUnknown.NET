namespace PlayerUnknown.Test
{
    using System;

    using PlayerUnknown.Cheats;
    using PlayerUnknown.Leaker;
    using PlayerUnknown.Logic.Weapons;
    using PlayerUnknown.Sniffer;

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

                if (PUBG.IsAttached)
                {
                    Logging.Info(typeof(Program), "PUBG is attached.");

                    // Program.TestLeaker();
                    // Program.TestSniffer();
                    // Program.TestNoRecoil();
                    Program.TestFastFire();
                }
                else
                {
                    Logging.Info(typeof(Program), "PUBG.IsAttached() != true at Main().");
                }
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
    }
}
