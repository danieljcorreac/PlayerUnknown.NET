namespace PlayerUnknown.Test
{
    using System;

    using PlayerUnknown.Leaker;
    using PlayerUnknown.Sniffer;

    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        internal static void Main(string[] Args)
        {
            if (Args.Length > 0)
            {
                Console.WriteLine("[*] Args : " + Args[0] + ".");
            }
            else
            {
                PUBG.Attach();
                PUBG.EnableEvents();

                if (PUBG.IsAttached)
                {
                    Program.TestLeaker();
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
        /// Tests the leaker.
        /// </summary>
        internal static void TestLeaker()
        {
            FuckBattlEye.Run("PlayerUnknown.Test.exe", "TslGame");
        }
    }
}
