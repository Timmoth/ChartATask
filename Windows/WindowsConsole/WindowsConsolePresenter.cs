using System;
using System.Collections.Generic;
using ChartATask.Core.Data;

namespace ChartATask.Presenters.Windows
{
    internal class WindowsConsolePresenter
    {
        public void Dispose()
        {
        }

        public void Update(List<IDataSet> dataSetCollection)
        {
            foreach (var dataSet in dataSetCollection)
            {
                Console.WriteLine(dataSet.ToString());
            }
        }
    }
}