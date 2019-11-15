using ChartATask.Core;
using ChartATask.Core.Persistence;
using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Requests;
using ChartATask.Interactors.Windows.Requests;
using ChartATask.Interactors.Windows.Watchers;
using System;
using System.Windows.Forms;

namespace ChartATask.Presenters.Windows
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.Run(new ChartATaskConsole());
        }

        private class ChartATaskConsole : ApplicationContext
        {
            public ChartATaskConsole()
            {
                Run();
                Application.Exit();
            }

            private static void Run()
            {
                var engine = new Engine(
                    new CsvPersistence("./"),
                    new EventWatcherManager(new EventWatcher[]
                    {
                        new WindowsKeyboardEventWatcher(),
                        new WindowsRunningAppEventWatcher(),
                        new WindowsAppTitleEventWatcher(),
                        new WindowsFocusedAppEventWatcher()
                    }),
                    new RequestEvaluatorManager(new[]
                    {
                        new WindowsAppRunningRequest()
                    }));

                MessageBox.Show("Click OK to close");

                engine.Dispose();
            }
        }
    }
}