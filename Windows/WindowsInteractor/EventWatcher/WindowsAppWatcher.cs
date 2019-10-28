using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using ChartATask.Core.Interactors.Watchers;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Models.Events.AppEvents;
using HWND = System.IntPtr;

namespace ChartATask.Interactors.Windows.EventWatcher
{
    public class WindowsAppWatcher : IAppWatcher
    {
        private readonly List<AppRunEvent> _appRunEvents;
        private readonly List<AppTitleEvent> _appTitleEvents;
        private readonly object _mutex = new object();

        private readonly ManagementEventWatcher _processStartEvent;
        private readonly ManagementEventWatcher _processStopEvent;
        private Dictionary<string, HashSet<string>> _appOpenWindows;

        public WindowsAppWatcher()
        {
            _appRunEvents = new List<AppRunEvent>();
            _appOpenWindows = new Dictionary<string, HashSet<string>>();

            _processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
            _processStartEvent.EventArrived += processStartEvent_EventArrived;

            _processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");
            _processStopEvent.EventArrived += processStopEvent_EventArrived;
        }

        public event EventHandler<IEvent> OnEvent;

        public void SetListeners(List<IEvent> events)
        {
            _appRunEvents.AddRange(events.OfType<AppRunEvent>());
        }

        public void Start()
        {
            try
            {
                lock (_mutex)
                {
                    UpdateOpenWindows();
                }

                _processStartEvent?.Start();
                _processStopEvent?.Start();
            }
            catch (Exception exception)
            {
            }
        }

        public void Stop()
        {
            _processStartEvent?.Stop();
            _processStopEvent?.Stop();
        }

        public void Dispose()
        {
            _processStartEvent?.Dispose();
            _processStopEvent?.Dispose();
        }

        private void processStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var processName = ProcessNameToString(e.NewEvent.Properties["ProcessName"].Value.ToString());

            lock (_mutex)
            {
                UpdateOpenWindows();
            }

            OnEvent?.Invoke(this, new AppRunEvent(processName, true));
        }

        private void processStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var processName = ProcessNameToString(e.NewEvent.Properties["ProcessName"].Value.ToString());

            lock (_mutex)
            {
                UpdateOpenWindows();
            }

            OnEvent?.Invoke(this, new AppRunEvent(processName, false));
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

            EnumWindows(delegate(HWND hWnd, int lParam)
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
        private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(HWND hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);
    }
}