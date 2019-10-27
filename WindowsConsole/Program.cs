using System;
using ChartATask.Core;
using ChartATask.Interactors.Windows;

namespace ChartATask.Presenters.Windows
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("ChartATask Started");

            var engine = new Engine(new WindowsConsolePresenter(), new WindowsSystemInteractor());
            engine.Start();

            while (Console.ReadLine()?.ToLower() != "exit")
            {
            }

            engine.Stop();
            engine.Dispose();

            Console.WriteLine("ChartATask Finished");
        }
    }
}