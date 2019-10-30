using System;
using System.Windows.Forms;
using ChartATask.Core;
using ChartATask.Core.Events;
using ChartATask.Core.Persistence;
using ChartATask.Core.Requests;
using ChartATask.Interactors.Windows.Requests;
using ChartATask.Interactors.Windows.Watchers;

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
                Console.WriteLine("ChartATask Started");

                Run();

                Console.WriteLine("ChartATask Finished");
                Application.Exit();
            }

            private static void Run()
            {
                var eventCollector = new EventWatcherManager();
                eventCollector.Register(new WindowsKeyboardEventWatcher());
                eventCollector.Register(new WindowsRunningAppEventWatcher());
                eventCollector.Register(new WindowsAppTitleEventWatcher());
                eventCollector.Register(new WindowsFocusedAppEventWatcher());

                var requestManager = new RequestEvaluatorManager();
                requestManager.Register(new WindowsAppRunningRequest());

                var engine = new Engine(
                    new CsvPersistence("./"),
                    eventCollector,
                    requestManager);

                engine.Start();

                MessageBox.Show("Click OK to close");

                engine.Dispose();
            }
        }
    }
}