using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpGL.WinForms
{
    public static class IdleTimeManager
    {
        static IdleTimeManager()
        {
            //  Hook the application idle time event.
            Application.Idle += Application_Idle;
        }

        static void Application_Idle(object sender, EventArgs e)
        {
            //  Single instance of the args and a single reference to the event for performance reasons.
            var args = new EventArgs();
            var handler = OnIdle;

            //  Until there are any messages to process, we can fire off idle time events.
            IntPtr messagePointer;
            while (PeekMessage(out messagePointer, IntPtr.Zero, 0, 0, 0) == false)
            {
                if (handler != null)
                    handler(null, args);
            }
        }

        public static event EventHandler OnIdle;

        [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool PeekMessage(out IntPtr msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
    }
}
