namespace PlayerUnknown.Test
{
    using System;

    using PlayerUnknown.Sniffer;

    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        internal static void Main()
        {
            PUBG.Attach();
            PUBG.EnableEvents();

            if (PUBG.IsAttached)
            {
                var PubgSniffer = new PubgSniffer();

                if (PubgSniffer.TryConfigure())
                {
                    PubgSniffer.StartCapture();
                }
                else
                {
                    Logging.Info(typeof(Program), "TryConfigure() != true at Main().");
                }
            }
            else
            {
                Logging.Info(typeof(Program), "PUBG.IsAttached() != true at Main().");
            }

            Console.ReadKey(false);
        }
    }
}
