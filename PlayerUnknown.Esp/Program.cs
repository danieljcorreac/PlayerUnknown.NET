namespace PlayerUnknown.Esp
{
    using System;

    using PlayerUnknown.Helpers;
    using PlayerUnknown.Native;

    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main(string[] Args)
        {
            PUBG.Initialize();
            PUBG.Attach();
            PUBG.EnableEvents();

            if (PUBG.IsAttached)
            {
                var Module = PUBG.MainModule;
                var Handle = Win32.OpenProcess(PUBG.AttachedProcess);

                if (Module != null)
                {
                    var Memory    = new Memory(Handle, IntPtr.Zero);
                    var UWorldRef = Memory.Read<UIntPtr>(Module.BaseAddress + 0x40D9B20);
                    var UWorld    = Memory.Read<UIntPtr>(UWorldRef);
                    var GInstance = Memory.Read<UIntPtr>(UWorld + 0x148);
                    var GNames    = Memory.Read<UIntPtr>(Module.BaseAddress + 0x414a878);

                    Console.WriteLine("[*] Done !");
                }
                else
                {
                    Console.WriteLine("Module is null.");
                }
            }
            else
            {
                Console.WriteLine("PUBG is not attached, bitch.");
            }

            Console.ReadKey(false);
        }
    }
}