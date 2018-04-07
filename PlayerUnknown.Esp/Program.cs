namespace PlayerUnknown.Esp
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    using PlayerUnknown.Helpers;
    using PlayerUnknown.Leaker;
    using PlayerUnknown.Offsets;

    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main(string[] Args)
        {
            if (Args.Length == 0)
            {
                Console.WriteLine(Path.GetFileName(Assembly.GetEntryAssembly().Location));
                FuckBattlEye.Run(Path.GetFileName(Assembly.GetEntryAssembly().Location), "TslGame");
                return;
            }

            Console.WriteLine("Leaked handle is " + Args[0] + ".");

            if (!uint.TryParse(Args[0], out uint Pointer))
            {
                throw new Exception("Pointer is incorrect.");
            }

            var Handle      = (IntPtr) Pointer;

            var Scanner     = new Scanner(Handle);

            PUBG.Attach();
            
            if (PUBG.IsAttached)
            {
                var Module = PUBG.MainModule;

                if (Module != null)
                {
                    var Memory      = new Memory(Handle, Module.BaseAddress);
                    var Selected    = Scanner.SelectModule(Module.BaseAddress, (uint) Module.ModuleMemorySize);

                    if (Selected)
                    {
                        var GWorldAddress       = (IntPtr) Scanner.FindPattern("48 8B 1D ? ? ? ? 74 40");

                        if (GWorldAddress != IntPtr.Zero)
                        {
                            var GWorldOffset    = Memory.Read<uint>(GWorldAddress + 3) + 7;
                            var ppUWorld        = (IntPtr) ((ulong) GWorldAddress + GWorldOffset);
                            var ptrUWorld       = Memory.Read<IntPtr>(ppUWorld);

                            Console.WriteLine($"Found UWorld at 0x{((ulong) ppUWorld):x2}.");
                        }
                        else
                        {
                            Console.WriteLine("UWorld Address not found.");
                        }

                        var GNamesAddress   = Scanner.FindPattern("48 89 1D ? ? ? ? 48 8B 5C 24 ? 48 83 C4 28 C3 48 8B 5C 24 ? 48 89 05 ? ? ? ? 48 83 C4 28 C3");

                        if (GNamesAddress != 0)
                        {
                            var GNamesOffset = Memory.Read<uint>((IntPtr)GNamesAddress + 3);

                            GNamesAddress += GNamesOffset + 7;

                            Console.WriteLine($"Found UNames at 0x{(GNamesAddress - 0):x2}.");
                        }
                        else
                        {
                            Console.WriteLine("UNames Address not found.");
                        }

                        float RangeMin = Memory.Read<float>((IntPtr) UQualitySliderWidget_C.RangeMin);
                        float RangeMax = Memory.Read<float>((IntPtr) UQualitySliderWidget_C.RangeMax);

                        Console.WriteLine("[*] Range : " + RangeMin + " - " + RangeMax + ".");

                        var Dump = Memory.DumpMemory();

                        Console.WriteLine("Memory has been dumped.");

                        File.WriteAllBytes("Dump.bin", Dump);
                    }
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

            /**************/
            /*
            IntPtr[] hMods      = new IntPtr[1024];
            var pModules        = GCHandle.Alloc(hMods, GCHandleType.Pinned);

            Console.WriteLine("hMods[0] = " + (ulong) hMods[0].ToInt64() + ".");

            uint size           = (uint) IntPtr.Size * 1024;
            uint cbNeeded;

            if (Win32.EnumProcessModules(Handle, pModules.AddrOfPinnedObject(), size, out cbNeeded))
            {
                Memory = new Memory(Handle, hMods[0]);

                int cb = Marshal.SizeOf(typeof(Win32._MODULEINFO));

                Win32._MODULEINFO modinfo;
                Win32.GetModuleInformation(Handle, hMods[0], out modinfo, cb);

                if (Scanner.SelectModule(hMods[0], modinfo.SizeOfImage))
                {
                    var GWorldAddress = (IntPtr) Scanner.FindPattern("48 8B 1D ? ? ? ? 74 40");

                    if (GWorldAddress != IntPtr.Zero)
                    {
                        var GWorldOffset    = Memory.Read<uint>(GWorldAddress + 3) + 7;
                        var ppUWorld        = (IntPtr)((ulong)GWorldAddress + GWorldOffset);
                        var pUWorld         = Memory.Read<IntPtr>(ppUWorld);

                        Console.WriteLine($"UWord Address found at 0x{((ulong) ppUWorld - (ulong) hMods[0]):x2}.");
                    }
                    else
                    {
                        Console.WriteLine("UWorld Address not found.");
                    }

                    var GNamesAddress   = (IntPtr) Scanner.FindPattern("48 89 1D ? ? ? ? 48 8B 5C 24 ? 48 83 C4 28 C3 48 8B 5C 24 ? 48 89 05 ? ? ? ? 48 83 C4 28 C3");

                    if (GNamesAddress != IntPtr.Zero)
                    {
                        var GNamesOffset    = Memory.Read<uint>(GNamesAddress + 3);
                        GNamesAddress       = (IntPtr) (GNamesAddress.ToInt64() + GNamesOffset + 7);

                        Console.WriteLine($"Found GNames at 0x{((ulong) GNamesAddress.ToInt64() - (ulong) hMods[0]):x2}.");
                    }
                    else
                    {
                        Console.WriteLine("UNames Address not found.");
                    }

                    Console.WriteLine("hMods[0] = " + (ulong) hMods[0].ToInt64() + ".");
                }
            }

            pModules.Free(); */

            /**************/

            Console.ReadKey(false);
        }

        private static class Win32
        {
            [DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            public static extern bool EnumProcessModules(IntPtr hProcess, [Out] IntPtr lphModule, uint cb, out uint lpcbNeeded);

            [DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out _MODULEINFO lpModInfo, int cb);


            [StructLayout(LayoutKind.Sequential)]
            public struct _MODULEINFO
            {
                public IntPtr lpBaseOfDll;
                public uint SizeOfImage;
                public IntPtr EntryPoint;
            }
        }
    }
}
