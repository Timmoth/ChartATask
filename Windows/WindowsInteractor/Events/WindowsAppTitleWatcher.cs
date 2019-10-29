using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChartATask.Core.Events.Watchers;
using ChartATask.Core.Models.Events.AppEvents;

namespace ChartATask.Interactors.Windows.Events
{
    public class WindowsAppTitleWatcher : IAppTitleWatcher
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        private Dictionary<string, HashSet<string>> _appOpenWindows;

        public WindowsAppTitleWatcher()
        {
            _appOpenWindows = new Dictionary<string, HashSet<string>>();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public event EventHandler<AppTitleEvent> OnEvent;

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        public void Start()
        {
            new TaskFactory(_cancellationTokenSource.Token).StartNew(async () =>
            {
                while (true)
                {
                    UpdateOpenWindows();
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private void UpdateOpenWindows()
        {
            var newAppWindowTitles = GetOpenWindows();

            foreach (var oldWindowTitles in _appOpenWindows.ToList())
            {
                if (!newAppWindowTitles.TryGetValue(oldWindowTitles.Key, out var newTitles))
                {
                    _appOpenWindows.Remove(oldWindowTitles.Key);
                    WindowTitleChange(oldWindowTitles.Key, oldWindowTitles.Value.ToList(), false);
                }
                else
                {
                    WindowTitleChange(oldWindowTitles.Key, oldWindowTitles.Value.Where(p => !newTitles.Contains(p)),
                        false);
                    WindowTitleChange(oldWindowTitles.Key, newTitles.Where(p => !oldWindowTitles.Value.Contains(p)),
                        true);
                }
            }

            foreach (var newlyOpenedApps in newAppWindowTitles.Where(p => !_appOpenWindows.ContainsKey(p.Key)))
            {
                WindowTitleChange(newlyOpenedApps.Key, newlyOpenedApps.Value, true);
            }

            _appOpenWindows = newAppWindowTitles;
        }

        private void WindowTitleChange(string name, IEnumerable<string> windowTitles, bool shown)
        {
            foreach (var windowTitle in windowTitles)
            {
                OnEvent?.Invoke(this, new AppTitleEvent(name, windowTitle, shown));
                Console.WriteLine(new AppTitleEvent(name, windowTitle, shown).ToString());
            }
        }

        private Dictionary<string, HashSet<string>> GetOpenWindows()
        {
            var shellWindow = GetShellWindow();
            var currentAppOpenWindows = new Dictionary<string, HashSet<string>>();

            EnumWindows(delegate(IntPtr hWnd, int lParam)
            {
                if (hWnd == shellWindow)
                {
                    return true;
                }

                if (!IsWindowVisible(hWnd))
                {
                    return true;
                }

                var length = GetWindowTextLength(hWnd);
                if (length == 0)
                {
                    return true;
                }

                var builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);
                GetWindowThreadProcessId(hWnd, out var processId);
                try
                {
                    var processName = ProcessNameToString(Process.GetProcessById((int) processId).ProcessName);

                    if (!currentAppOpenWindows.TryGetValue(processName, out var titles))
                    {
                        titles = new HashSet<string>();
                        currentAppOpenWindows[processName] = titles;
                    }

                    titles.Add(builder.ToString());
                }
                catch
                {
                }

                return true;
            }, 0);

            return currentAppOpenWindows;
        }

        private static string ProcessNameToString(string processName)
        {
            return processName.ToLower().Split('.')[0];
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