using System;
using System.Threading;
using ChartATask.Core;
using ChartATask.Interactors.Windows;

namespace ChartATask.Presenters.Windows
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("ChartATask Started");

            var cancellationTokenSource = new CancellationTokenSource();

            var engine = new Engine(new WindowsConsolePresenter(), new WindowsInteractor());

            engine.Run(cancellationTokenSource).Wait();

            Console.WriteLine("ChartATask Finished");
        }
    }
}