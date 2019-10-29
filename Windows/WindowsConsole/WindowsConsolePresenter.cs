using System;
using ChartATask.Core.Models;
using ChartATask.Core.Presenter;

namespace ChartATask.Presenters.Windows
{
    internal class WindowsConsolePresenter : IPresenter
    {
        public void Update(DataSetCollection dataSetCollection)
        {
            foreach (var dataSet in dataSetCollection.DataSets)
            {
                Console.WriteLine(dataSet.ToString());
            }

            ClearLastLine();
        }

        public void Dispose()
        {
        }

        public static void ClearLastLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
}