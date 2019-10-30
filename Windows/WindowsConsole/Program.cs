using System;
using System.Windows.Forms;
using ChartATask.Core;
using ChartATask.Core.Events;
using ChartATask.Core.Persistence;
using ChartATask.Core.Requests;
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

        internal class ChartATaskConsole : ApplicationContext
        {
            public ChartATaskConsole()
            {
                Console.WriteLine("ChartATask Started");

                var eventCollector = new EventWatchers();
                eventCollector.Register(new WindowsKeyboardWatcher());
                eventCollector.Register(new WindowsRunningAppWatcher());
                eventCollector.Register(new WindowsAppTitleWatcher());

                var engine = new Engine(new CsvPersistence(), new WindowsConsolePresenter(), eventCollector,
                    new RequestEvaluator());
                engine.Start();
                engine.Load(@"./data.csv");

                MessageBox.Show("Click OK to close");

                engine.Stop();
                engine.Save();

                engine.Dispose();
                Console.WriteLine("ChartATask Finished");
                Application.Exit();
            }

            private static void Run()
            {
            }
        }
    }
}