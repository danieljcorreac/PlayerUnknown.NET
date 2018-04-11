namespace PlayerUnknown.Test
{
    using System;

    using WinApi.Gdi32;
    using WinApi.User32;
    using WinApi.Windows;
    using WinApi.Windows.Controls;

    public class TestWindow : Window
    {
        protected override void OnPaint(ref PaintPacket Packet)
        {
            IntPtr Handle = this.BeginPaint(out var Paint);

            Logging.Info(this.GetType(), "H : " + Paint.PaintRect.Height);
            Logging.Info(this.GetType(), "W : " + Paint.PaintRect.Width);

            if (Handle != IntPtr.Zero)
            {
                User32Methods.FillRect(Handle, ref Paint.PaintRect, Gdi32Helpers.GetStockObject(StockObject.WHITE_BRUSH));
                User32Methods.DrawText(Handle, "Rekt", 4, ref Paint.PaintRect, 2);
            }
            else
            {
                Logging.Warning(this.GetType(), "Handle was null at OnPaint().");
            }

            this.EndPaint(ref Paint);
            base.OnPaint(ref Packet);
        }
    }
}
