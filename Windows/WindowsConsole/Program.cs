using System;
using System.Collections.Generic;
using ChartATask.Core;
using ChartATask.Core.Models;
using ChartATask.Core.Persistence;
using ChartATask.Interactors.Windows;

namespace ChartATask.Presenters.Windows
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("ChartATask Started");
            var persistence = new CSVPersistence();

            var dataSetCollection = persistence.Load();
            //var dataSetCollection = new DataSetCollection(new List<IDataSet>
            //{
            //    new AppSessionDataSet(@"firefox", @"GitHub - Mozilla Firefox"),
            //    new AppSessionDataSet(@"devenv", @"chartatask (Running) - Microsoft Visual Studio (Administrator)"),
            //});

            var engine = new Engine(new WindowsConsolePresenter(), new WindowsSystemInteractor(), dataSetCollection);
            engine.Start();

            while (Console.ReadLine()?.ToLower() != "exit")
            {

            }

            engine.Stop();
            engine.Dispose();


            persistence.Save(dataSetCollection);

            Console.WriteLine("ChartATask Finished");
        }
    }
}