namespace PlayerUnknown
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using PlayerUnknown.Native.Enums;
    using PlayerUnknown.Native.Structures;

    public static class PUBG
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PUBG"/> is initiliazed.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the attached <see cref="PUBG"/> process.
        /// </summary>
        public static Process AttachedProcess
        {
            get
            {
                if (PUBG._AttachedProcess != null)
                {
                    if (PUBG._AttachedProcess.HasExited == false)
                    {
                        return PUBG._AttachedProcess;
                    }

                    PUBG._AttachedProcess        = null;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is attached.
        /// </summary>
        public static bool IsAttached
        {
            get
            {
                return PUBG.AttachedProcess != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is detached.
        /// </summary>
        public static bool IsDetached
        {
            get
            {
                return PUBG.AttachedProcess == null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is running.
        /// </summary>
        public static bool IsRunning
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    if (PUBG._AttachedProcess.HasExited == false)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is responding.
        /// </summary>
        public static bool IsResponding
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    return PUBG._AttachedProcess.Responding;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is minimized.
        /// </summary>
        public static bool IsMinimized
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    var Placement = Native.Window.GetWindowPlacement(PUBG._AttachedProcess.MainWindowHandle);

                    if (Placement.ShowCmd == WindowStates.ShowMinimized || Placement.ShowCmd == WindowStates.ForceMinimized)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is maximized.
        /// </summary>
        public static bool IsMaximized
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    var Placement = Native.Window.GetWindowPlacement(PUBG._AttachedProcess.MainWindowHandle);

                    if (Placement.ShowCmd == WindowStates.Maximize || Placement.ShowCmd == WindowStates.ShowMaximized)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is displayed on the screen.
        /// </summary>
        public static bool IsOnScreen
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    var Placement = Native.Window.GetWindowPlacement(PUBG._AttachedProcess.MainWindowHandle);
                    var Flag      = Placement.ShowCmd;

                    if (PUBG.IsMaximized)
                    {
                        return true;
                    }

                    if (PUBG.IsMinimized)
                    {
                        return false;
                    }

                    if (Flag == WindowStates.Restore || Flag == WindowStates.Show || Flag == WindowStates.ShowNormal)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the <see cref="PUBG"/> window properties.
        /// </summary>
        public static WindowPlacement Window
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    return Native.Window.GetWindowPlacement(PUBG._AttachedProcess.MainWindowHandle);
                }

                return new WindowPlacement();
            }
        }

        /// <summary>
        /// Gets the <see cref="PUBG"/> window rectangle.
        /// </summary>
        public static Rectangle WindowRec
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    return Native.Window.GetWindowRectangle(PUBG._AttachedProcess.MainWindowHandle);
                }

                return new Rectangle();
            }
        }

        /// <summary>
        /// Gets the <see cref="PUBG"/> window handle.
        /// </summary>
        public static IntPtr WindowHandle
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    return PUBG._AttachedProcess.MainWindowHandle;
                }

                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets the <see cref="PUBG"/> modules properties.
        /// </summary>
        public static List<ProcessModule> Modules
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    return PUBG._AttachedProcess.Modules.Cast<ProcessModule>().ToList();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="PUBG"/> main module propertie.
        /// </summary>
        public static ProcessModule MainModule
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    return PUBG._AttachedProcess.MainModule;
                }

                return null;
            }
        }

        private static Process      _AttachedProcess;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (PUBG.Initialized)
            {
                return;
            }

            // TODO

            PUBG.Initialized = true;
        }

        /// <summary>
        /// Attaches this instance to <see cref="PUBG"/>.
        /// </summary>
        /// <param name="BattlEyeProtected">If it must be protected or not.</param>
        public static void Attach(bool BattlEyeProtected = false)
        {
            var Processes = Process.GetProcessesByName(BattlEyeProtected ? "TslGame_BE" : "TslGame");
            var Processus = (Process) null;

            if (Processes.Length == 0)
            {
                // throw new ProcessNotFoundException("Processes.Length == 0 at PUBG.Attach().");
            }
            else
            {
                Processus = Processes[0];

                if (Processes.Length > 1)
                {
                    Logging.Info(typeof(PUBG), "Processes.Length > 1 at PUBG.Attach().");

                    foreach (var Match in Processes)
                    {
                        // Get the correct instance.
                    }
                }
            }

            if (Processus != null)
            {
                PUBG._AttachedProcess        = Processus;
            }
        }

        /// <summary>
        /// Detaches this instance to <see cref="PUBG"/>.
        /// </summary>
        public static void Detach()
        {
            if (PUBG._AttachedProcess == null)
            {
                Logging.Info(typeof(PUBG), "_AttachedProcess == null at PUBG.Detach().");
            }

            PUBG._AttachedProcess        = null;
        }

        /// <summary>
        /// Enables the events.
        /// </summary>
        public static void EnableEvents()
        {
            Events.EventHandlers.Run().ConfigureAwait(false);
        }
    }
}
