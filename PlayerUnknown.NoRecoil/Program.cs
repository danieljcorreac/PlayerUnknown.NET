namespace PlayerUnknown.NoRecoil
{
    using System;

    using PlayerUnknown.Cheats;
    using PlayerUnknown.Logic.Weapons;

    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            PUBG.Initialize();
            PUBG.Attach();
            PUBG.EnableEvents();

            if (PUBG.IsAttached && PUBG.IsRunning)
            {
                NoRecoil.Weapon = new Ak47();
                // NoRecoil.Weapon = new TommyGun();
            }

            NoRecoil.Run().Wait();
        }
    }
}
