namespace PlayerUnknown.Recoil
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Gma.System.MouseKeyHook;

    using PlayerUnknown.Native;
    using PlayerUnknown.Recoil.Logic;

    public static class NoRecoil
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NoRecoil"/> is initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="NoRecoil"/> is enabled.
        /// </summary>
        public static bool IsEnabled
        {
            get
            {
                return NoRecoil.HasLeftClick /* && NoRecoil.HasRightClick */;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has left click.
        /// </summary>
        public static volatile bool HasLeftClick;

        /// <summary>
        /// Gets or sets a value indicating whether this instance has right click.
        /// </summary>
        public static volatile bool HasRightClick;

        /// <summary>
        /// Gets or sets the locker.
        /// </summary>
        public static object Lock;

        /// <summary>
        /// Gets or sets the <see cref="Player"/>, currently playing the game.
        /// </summary>
        public static Weapon Weapon;

        /// <summary>
        /// Gets or sets the <see cref="Random"/> used to make the <see cref="NoRecoil"/> a bit random.
        /// </summary>
        public static Random Random;

        /// <summary>
        /// Enables the no recoil.
        /// </summary>
        public static async Task Run()
        {
            if (NoRecoil.Initialized)
            {
                return;
            }

            NoRecoil.Initialized = true;
            NoRecoil.Random      = new Random();
            NoRecoil.Lock        = new object();

            NoRecoil.Configure();

            while (true)
            {
                if (NoRecoil.IsEnabled && PUBG.IsOnScreen)
                {
                    if (NoRecoil.Weapon != null)
                    {
                        if (NoRecoil.Weapon.IsRecoilEnabled)
                        {
                            await NoRecoil.DoRecoil();

                            if (NoRecoil.Weapon.FireRate > 0)
                            {
                                await Task.Delay(NoRecoil.Weapon.FireRate);
                            }
                            else
                            {
                                Logging.Warning(typeof(NoRecoil), "FireRate == 0.");
                            }
                        }
                        else
                        {
                            Logging.Warning(typeof(NoRecoil), "IsRecoilEnabled == false.");
                        }
                    }
                    else
                    {
                        Logging.Warning(typeof(NoRecoil), "Weapon == null.");
                    }
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        private static void Configure()
        {
            IMouseEvents GlobalHook     = Hook.GlobalEvents();

            GlobalHook.MouseDownExt    += OnMouseClick;
            GlobalHook.MouseUpExt      += OnMouseClick;
        }

        /// <summary>
        /// Globals the hook mouse down ext.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The args.</param>
        private static void OnMouseClick(object Sender, MouseEventExtArgs Args)
        {
            lock (NoRecoil.Lock)
            {
                if ((Args.Button & MouseButtons.Left) != 0)
                {
                    // Volatile.Write(ref NoRecoil.HasLeftClick, Args.IsMouseButtonDown);
                    NoRecoil.HasLeftClick = Args.IsMouseButtonDown;
                }

                if ((Args.Button & MouseButtons.Right) != 0)
                {
                    // Volatile.Write(ref NoRecoil.HasRightClick, Args.IsMouseButtonDown);
                    NoRecoil.HasRightClick = Args.IsMouseButtonDown;
                }
            }
        }

        /// <summary>
        /// Does the recoil hack and move the mouse with some randomness.
        /// </summary>
        /// <param name="RecoilRate">The recoil rate.</param>
        /// <param name="Smooth">If set to true, moves the mouse pixel per pixel.</param>
        private static async Task DoRecoil(bool Smooth = false)
        {
            var Randomness      = (1 * NoRecoil.Weapon.RandomnessMultiplier);

            var DiffX           = NoRecoil.Random.Next(-Randomness, Randomness + 1);
            var DiffY           = NoRecoil.Random.Next(NoRecoil.Weapon.RecoilRate, (NoRecoil.Weapon.RecoilRate * 2) + 1);

            var TargetX         = DiffX;
            var TargetY         = DiffY;

            if (Smooth)
            {
                while (true)
                {
                    Mouse.MovePosition((DiffX > 0 ? +1 : -1), (DiffY > 0 ? +1 : -1));

                    if (TargetX > 0)
                    {
                        TargetX = TargetX - 1;
                    }
                    else if (TargetX < 0)
                    {
                        TargetX = TargetX + 1;
                    }

                    if (TargetY > 0)
                    {
                        TargetY = TargetY - 1;
                    }
                    else if (TargetY < 0)
                    {
                        TargetY = TargetY + 1;
                    }

                    if (TargetX == 0 && TargetY == 0)
                    {
                        break;
                    }

                    int Delay = (NoRecoil.Weapon.FireRate / 5);

                    if (Delay >= 10)
                    {
                        await Task.Delay(Delay);
                    }
                }
            }
            else
            {
                Mouse.MovePosition(DiffX, DiffY);
            }
        }
    }
}
