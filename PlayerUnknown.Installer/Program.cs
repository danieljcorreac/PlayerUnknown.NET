namespace PlayerUnknown.Installer
{
    using System;
    using System.IO;
    using System.Threading;

    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        public static void Main(string[] Args)
        {
            bool Uninstall = false;

            if (Args.Length > 0)
            {
                string Argument = Args[0];

                if (Argument == "-uninstall")
                {
                    Uninstall = true;
                }
            }

            if (Uninstall)
            {
                Program.Uninstall();
            }
            else
            {
                Program.Install();
            }

            Console.ReadKey(false);
        }

        /// <summary>
        /// Installs this program.
        /// </summary>
        public static void Install()
        {
            Console.Title   = "PlayerUnknown.Installer - Installing..";

            var Internals   = Directory.GetFiles("SysInternals");
            var SysPath     = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            var Percentage  = 0;

            Console.WriteLine("[*] PlayerUnknown's Battlegrounds");
            Console.WriteLine("[*] Installing internals..");
            Console.WriteLine();

            for (var I = 0; I < Internals.Length; I++)
            {
                var NewPercentage = (I + 1) * 15 / Internals.Length;

                if (Percentage < NewPercentage)
                {
                    var Difference = NewPercentage - Percentage;
                }

                Percentage = NewPercentage;

                var FileName = Path.GetFileName(Internals[I]);
                var FileCopy = Path.Combine(SysPath, FileName);

                Console.Write("[*] Copying '" + FileName + "' to '" + SysPath + "'.. ");

                if (File.Exists(FileCopy) == false)
                {
                    File.Copy(Internals[I], FileCopy);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("[FAILED]");
                    Console.ResetColor();
                }

                Console.Write(Environment.NewLine);

                Thread.Sleep(50);
            }

            Console.WriteLine();
            Console.Write("[*] Patching kernel.. ");

            /* if (File.Exists("SysInternals/Kernel/patch.exe"))
            {
                var Infos  = new ProcessStartInfo("SysInternals/Kernel/patch.exe")
                {
                    CreateNoWindow = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false
                };

                var Patcher = Process.Start(Infos);

                Thread.Sleep(1000);

                while (true)
                {
                    var PatcherOuput = Patcher.StandardOutput.ReadLine();

                    if (string.IsNullOrEmpty(PatcherOuput))
                    {
                        break;
                    }

                    Console.WriteLine("[*] " + PatcherOuput);
                }
            }
            else */
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("[FAILED]");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Uninstalls this program.
        /// </summary>
        public static void Uninstall()
        {
            var Internals   = Directory.GetFiles("SysInternals");
            var SysPath     = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            var Percentage  = 0;

            Console.WriteLine("[*] PlayerUnknown's Battlegrounds");
            Console.WriteLine("[*] Uninstalling internals..");
            Console.WriteLine();

            for (var I = 0; I < Internals.Length; I++)
            {
                var NewPercentage = (I + 1) * 15 / Internals.Length;

                if (Percentage < NewPercentage)
                {
                    var Difference = NewPercentage - Percentage;
                }

                Percentage = NewPercentage;

                var FileName = Path.GetFileName(Internals[I]);
                var FileCopy = Path.Combine(SysPath, FileName);

                Console.Write("[*] Deleting '" + FileName + "' from '" + SysPath + "'.. ");

                if (File.Exists(FileCopy))
                {
                    File.Delete(FileCopy);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("[FAILED]");
                    Console.ResetColor();
                }

                Console.Write(Environment.NewLine);

                Thread.Sleep(50);
            }
        }
    }
}
