namespace PlayerUnknown.Cheats
{
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Gma.System.MouseKeyHook;

    using PlayerUnknown.Native;

    public static class FastFire
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FastFire"/> is initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance can fast fire.
        /// </summary>
        public static bool CanFastFire
        {
            get
            {
                return FastFire.HasLeftClick;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can bunny hop.
        /// </summary>
        public static bool CanBunnyHop
        {
            get
            {
                return FastFire.HasSpacePushed;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has left click.
        /// </summary>
        public static volatile bool HasLeftClick;

        /// <summary>
        /// Gets or sets a value indicating whether this instance has space pushed.
        /// </summary>
        public static volatile bool HasSpacePushed;

        /// <summary>
        /// Gets or sets the mouse locker.
        /// </summary>
        public static object LockMouse;

        /// <summary>
        /// Gets or sets the keyboard locker.
        /// </summary>
        public static object LockKeyboard;

        /// <summary>
        /// Enables the no recoil.
        /// </summary>
        public static async Task Run()
        {
            if (FastFire.Initialized)
            {
                return;
            }

            FastFire.Initialized    = true;
            FastFire.LockMouse      = new object();
            FastFire.LockKeyboard   = new object();

            FastFire.Configure();

            while (FastFire.Initialized)
            {
                if (PUBG.IsOnScreen)
                {
                    if (FastFire.CanFastFire)
                    {
                        FastFire.DoFire();
                    }

                    if (FastFire.CanBunnyHop)
                    {
                        FastFire.DoJump();
                    }

                    await Task.Delay(1);
                }
                else
                {
                    await Task.Delay(25);
                }
            }
        }

        /// <summary>
        /// Hooks the <see cref="Mouse"/>.
        /// </summary>
        private static void Configure()
        {
            IMouseEvents GlobalHook = Hook.GlobalEvents();

            GlobalHook.MouseDownExt += FastFire.OnMouseClick;
            GlobalHook.MouseUpExt   += FastFire.OnMouseClick;

            IKeyboardEvents GlobalKHook = Hook.GlobalEvents();

            GlobalKHook.KeyDown     += FastFire.OnKeyPushed;
            GlobalKHook.KeyUp       += FastFire.OnKeyReleased;
            
        }

        /// <summary>
        /// Globals the hook mouse down ext.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The args.</param>
        private static void OnMouseClick(object Sender, MouseEventExtArgs Args)
        {
            lock (FastFire.LockMouse)
            {
                if (Args.Button == MouseButtons.Left)
                {
                    FastFire.HasLeftClick = Args.IsMouseButtonDown;
                }
            }
        }

        /// <summary>
        /// Called when a key is pushed.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private static void OnKeyPushed(object Sender, KeyEventArgs Args)
        {
            lock (FastFire.LockKeyboard)
            {
                // Logging.Info(typeof(FastFire), Args.KeyCode.ToString() + " has been pushed.");

                if (Args.KeyCode == Keys.Space)
                {
                    // Logging.Info(typeof(FastFire), "Space has been pushed.");
                    FastFire.HasSpacePushed = true;
                }
            }
        }

        /// <summary>
        /// Called when a key is released.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private static void OnKeyReleased(object Sender, KeyEventArgs Args)
        {
            lock (FastFire.LockKeyboard)
            {
                // Logging.Info(typeof(FastFire), Args.KeyCode.ToString() + " has been released.");

                if (Args.KeyCode == Keys.Space)
                {
                    // Logging.Info(typeof(FastFire), "Space has been released.");
                    FastFire.HasSpacePushed = false;
                }
            }
        }

        /// <summary>
        /// Does the fast fire and calls the mouse event.
        /// </summary>
        private static void DoFire()
        {
            Mouse.Click();
        }

        /// <summary>
        /// Does the jump and calls the key event.
        /// </summary>
        private static void DoJump()
        {
            SendKeys.SendWait(" ");
        }
    }
}
