namespace PlayerUnknown.FastFire
{
    using System;
    using System.Threading;

    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Program.SetHook();

            new Thread(() =>
            {
                PUBG.Initialize();
                PUBG.Attach();
                PUBG.EnableEvents();

                if (PUBG.IsAttached && PUBG.IsRunning)
                {
                    FastFire.Run().Wait();
                }

            }).Start();
        }
    }
}
