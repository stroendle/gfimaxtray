using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace gfimaxtray
{
    class clsNotifyBox
    {
        private Form m_Notify;

        public void ShowBox(object message)
        {
            m_Notify = new Form();
            m_Notify.FormBorderStyle = FormBorderStyle.None;
            m_Notify.AllowTransparency = true;
            m_Notify.BackColor = Color.Red;
            //m_Notify.TransparencyKey = m_Notify.BackColor;
            m_Notify.ShowInTaskbar = false;

            Label textLabel = new Label();
            textLabel.Text = (string)message;
            textLabel.Font = new Font(FontFamily.GenericSansSerif, 20.0f, FontStyle.Regular);
            Graphics textGraphics = m_Notify.CreateGraphics();
            textLabel.Size = textGraphics.MeasureString((string)message, textLabel.Font).ToSize();
            textLabel.Location = new Point(0, 0);

            m_Notify.Controls.Add(textLabel);
            m_Notify.Size = textLabel.Size;

            m_Notify.StartPosition = FormStartPosition.Manual;
            m_Notify.DesktopLocation = new Point(Screen.PrimaryScreen.Bounds.Width - m_Notify.Size.Width,
                                           Screen.PrimaryScreen.Bounds.Height - WindowHelper.GetTaskbarHeight() - m_Notify.Size.Height);

            m_Notify.Show();
            Application.DoEvents();
            Thread.Sleep(10000);
            m_Notify.Close();
        }
    }

    class WindowHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        public static int GetTaskbarHeight()
        {
            IntPtr handleTaskbar = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", "");
            if (handleTaskbar == IntPtr.Zero) return (0);

            Rectangle taskbarRect;
            GetWindowRect(handleTaskbar, out taskbarRect);

            return (taskbarRect.Height - taskbarRect.Y);
        }
    }
}
