using System;
using System.Collections.Generic;
using ChartATask.Core;
using ChartATask.Core.Models;
using ChartATask.Interactors.Windows;

namespace ChartATask.Presenters.Windows
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("ChartATask Started");
            var dataSetCollection = new DataSetCollection(new List<IDataSet>
            {
                new AppSessionDataSet(@"devenv", @"chartatask (Running) - Microsoft Visual Studio (Administrator)"),
                new AppSessionDataSet(@"firefox", @"GitHub - Mozilla Firefox")
            });

            var engine = new Engine(new WindowsConsolePresenter(), new WindowsSystemInteractor(), dataSetCollection);
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