namespace PlayerUnknown.Radar
{
    using System;
    using System.Threading;

    using PlayerUnknown.Radar.Logic;

    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
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

            Logging.Warning(typeof(PUBG), "PUBG is ready to proceed.");

            // ...

            var Radar = new Radar();

            // ...

            Console.ReadKey(false);
        }
    }
}