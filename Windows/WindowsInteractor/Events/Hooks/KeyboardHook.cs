using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ChartATask.Interactors.Windows.Events.Hooks
{
    public delegate void KeyPressed(int keyCode);

    internal class KeyboardHook
    {
        private const int WhKeyboardLl = 13;
        private const int WmKeydown = 0x0100;
        private static readonly LowLevelKeyboardProc Proc = HookCallback;
        private static IntPtr _hookId = IntPtr.Zero;
        public static event KeyPressed OnKeyPressed;

        public static void Start()
        {
            _hookId = SetHook(Proc, WhKeyboardLl);
        }

        public static void End()
        {
            UnhookWindowsHookEx(_hookId);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc, int hookID)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                if (curModule != null)
                {
                    return SetWindowsHookEx(hookID, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }

            return IntPtr.Zero;
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr) WmKeydown)
            {
                OnKeyPressed?.Invoke(Marshal.ReadInt32(lParam));
            }

            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod,
            uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}