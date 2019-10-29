using System;
using System.Windows.Forms;
using ChartATask.Core;
using ChartATask.Core.Events;
using ChartATask.Core.Persistence;
using ChartATask.Core.Requests;
using ChartATask.Interactors.Windows.Events;

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

                var persistence = new CSVPersistence();

                var dataSetCollection = persistence.Load();

                var eventCollector = new EventWatchers();
                eventCollector.Register(new WindowsKeyboardWatcher());
                eventCollector.Register(new WindowsRunningAppWatcher());
                eventCollector.Register(new WindowsAppTitleWatcher());

                var engine = new Engine(new WindowsConsolePresenter(), eventCollector, new RequestEvaluator(),
                    dataSetCollection);
                engine.Start();

                while (Console.ReadLine()?.ToLower() != "exit")
                {
                }

                engine.Stop();
                engine.Dispose();

                persistence.Save(dataSetCollection);
                persistence.Dispose();

                Console.WriteLine("ChartATask Finished");
            }
        }
    }
}