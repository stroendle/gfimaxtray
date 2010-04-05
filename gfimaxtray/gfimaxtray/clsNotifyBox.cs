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
            // Erstellen der Form welche die Benachrichtigung einblendet
            m_Notify = new Form();
            m_Notify.FormBorderStyle = FormBorderStyle.None;
            m_Notify.AllowTransparency = true;
            m_Notify.BackColor = Color.Magenta;
            m_Notify.TransparencyKey = m_Notify.BackColor;
            m_Notify.ShowInTaskbar = false;

            // Label mit Mitteilung
            Label textLabel = new Label();
            textLabel.Text = (string)message;
            textLabel.Font = new Font(FontFamily.GenericSansSerif, 20.0f, FontStyle.Regular);
            Graphics textGraphics = m_Notify.CreateGraphics();
            textLabel.Size = textGraphics.MeasureString((string)message, textLabel.Font).ToSize();
            textLabel.Location = new Point(0, 0);

            m_Notify.Controls.Add(textLabel);
            m_Notify.Size = textLabel.Size;

            // Herausfinden an welche Position wir die Form setzen müssen, damit diese überhalb der Tray eingeblendet wird
            m_Notify.StartPosition = FormStartPosition.Manual;
            m_Notify.DesktopLocation = new Point(Screen.PrimaryScreen.Bounds.Width - m_Notify.Size.Width,
                                           Screen.PrimaryScreen.Bounds.Height - WindowHelper.GetTaskbarHeight() - m_Notify.Size.Height);

            // Quick and Dirt: Da wir anschliessend den Thread anhalten, müssen wir noch alle Window Messages
            // abarbeiten, damit unsere NotifyBox richtig gezeichnet wird.
            m_Notify.Show();
            Application.DoEvents();
            Thread.Sleep(1000);
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
