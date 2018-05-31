namespace PlayerUnknown.Native
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;

    using Process.NET.Native.Types;

    using Bitmap    = System.Drawing.Bitmap;
    using Rectangle = Process.NET.Native.Types.Rectangle;

    public static class Window
    {
        /// <summary>
        /// Gets the window placement using the specified handle.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        public static WindowPlacement GetWindowPlacement(IntPtr Handle)
        {
            var Placement = new WindowPlacement();

            if (Win32.GetWindowPlacement(Handle, ref Placement))
            {
                return Placement;
            }

            return Placement;
        }

        /// <summary>
        /// Gets the window rectangle using the specified handle.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        public static Rectangle GetWindowRectangle(IntPtr Handle, bool RemoveBorders = true)
        {
            if (Win32.GetWindowRect(Handle, out Rectangle Rectangle))
            {
                if (RemoveBorders)
                {
                    Rectangle.Top    += (30 + 1);
                    Rectangle.Left   += (7 + 1);
                    Rectangle.Right  -= (7 + 1);
                    Rectangle.Bottom -= (7 + 1);
                }

                return Rectangle;
            }

            return Rectangle;
        }

        /// <summary>
        /// Finds the specified window.
        /// </summary>
        /// <param name="ClassName">Name of the class.</param>
        /// <param name="WindowsName">Name of the windows.</param>
        public static IntPtr FindWindow(string ClassName, string WindowsName = null)
        {
            return Win32.FindWindow(ClassName, WindowsName);
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        /// <param name="Title">The title.</param>
        public static bool SetTitle(IntPtr Handle, string Title)
        {
            return Win32.SetWindowText(Handle, Title);
        }

        /// <summary>
        /// Gets a screenshot of the game.
        /// </summary>
        public static Bitmap GetScreenshot(IntPtr Handle)
        {
            var Rectangle = GetWindowRectangle(Handle);

            if (Rectangle.Width == 0 && Rectangle.Height == 0)
            {
                return new Bitmap(0, 0);
            }

            var Bitmap = new Bitmap(Rectangle.Width, Rectangle.Height);
            
            using (Graphics G = Graphics.FromImage(Bitmap))
            {
                Win32.PrintWindow(Handle, G.GetHdc(), 1);
            }

            return Bitmap;
        }

        /// <summary>
        /// Gets the game screen pixels.
        /// </summary>
        public static Bitmap GetGamePixels(IntPtr Handle, bool RemoveBorders = true)
        {
            Win32.SetForegroundWindow(Handle);

            var Game    = GetWindowRectangle(Handle, RemoveBorders);
            var Pixels  = new Bitmap(Game.Width, Game.Height);
            var Drawing = Graphics.FromImage(Pixels);

            System.Threading.Thread.Sleep(50);
            
            using (Drawing)
            {
                Drawing.CopyFromScreen(Game.Left, Game.Top, 0, 0, Pixels.Size);
            }

            return Pixels;
        }

        /// <summary>
        /// Gets the game screen pixels asynchronously.
        /// </summary>
        public static async Task<Bitmap> GetGamePixelsAsync(IntPtr Handle, bool RemoveBorders = true)
        {
            Win32.SetForegroundWindow(Handle);

            var Game    = GetWindowRectangle(Handle, RemoveBorders);
            var Pixels  = new Bitmap(Game.Width, Game.Height);
            var Drawing = Graphics.FromImage(Pixels);

            await Task.Delay(50);
            
            using (Drawing)
            {
                Drawing.CopyFromScreen(Game.Left, Game.Top, 0, 0, Pixels.Size);
            }

            return Pixels;
        }
    }
}
