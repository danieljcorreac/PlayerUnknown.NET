namespace PlayerUnknown.Radar
{
    using System;
    using System.Threading.Tasks;

    using PlayerUnknown.Radar.Enums;
    using PlayerUnknown.Radar.Logic;

    internal static class Program
    {
        /// <summary>
        /// Gets a value indicating whether this instance is checking.
        /// </summary>
        public static bool IsChecking
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static async Task Main()
        {
            PUBG.Initialize();
            PUBG.Attach();
            PUBG.EnableEvents();

            if (IsChecking && PUBG.IsAttached == false)
            {
                Logging.Info(typeof(Program), "Waiting for PUBG to start...");

                while (PUBG.IsAttached != true)
                {
                    await Task.Delay(500);
                }

                await Task.Delay(2500);
            }

            Logging.Info(typeof(PUBG), "PUBG is started, ready to proceed.");

            // ...

            var Radar = new Radar(RadarMode.UseNetwork);
            Radar.Start();

            // ...

            Console.ReadKey(false);
        }
    }
}