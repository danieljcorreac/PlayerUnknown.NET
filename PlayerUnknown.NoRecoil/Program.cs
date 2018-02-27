﻿namespace PlayerUnknown.NoRecoil
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    using Gma.System.MouseKeyHook;

    using PlayerUnknown.Logic;
    using PlayerUnknown.Logic.Weapons;

    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Program.SetHook();

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

        /// <summary>
        /// Hooks the <see cref="Mouse"/>.
        /// </summary>
        public static void SetHook()
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
        public static void OnMouseClick(object Sender, MouseEventExtArgs Args)
        {
            if ((Args.Button & MouseButtons.Left) != 0)
            {
                Volatile.Write(ref NoRecoil.HasLeftClick, Args.IsMouseButtonDown);
            }

            if ((Args.Button & MouseButtons.Right) != 0)
            {
                Volatile.Write(ref NoRecoil.HasRightClick, Args.IsMouseButtonDown);
            }
        }
    }
}
