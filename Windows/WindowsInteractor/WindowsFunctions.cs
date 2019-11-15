using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ChartATask.Interactors.Windows
{
    internal static class WindowsFunctions
    {
        public static string GetProcessName(IntPtr hWnd)
        {
            GetWindowThreadProcessId(hWnd, out var processId);
            try
            {
                return Process.GetProcessById((int)processId).ProcessName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetWindowTitle(IntPtr hWnd)
        {
            var shellWindow = GetShellWindow();
            if (hWnd == shellWindow)
            {
                return string.Empty;
            }

            if (!IsWindowVisible(hWnd))
            {
                return string.Empty;
            }

            var length = GetWindowTextLength(hWnd);
            if (length == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder(length);
            GetWindowText(hWnd, builder, length + 1);
            return builder.ToString();
        }

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);
    }
}