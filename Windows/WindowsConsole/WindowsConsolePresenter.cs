using System;
using System.Collections.Generic;
using ChartATask.Core;
using ChartATask.Core.Data;
using ChartATask.Core.Presenter;

namespace ChartATask.Presenters.Windows
{
    internal class WindowsConsolePresenter : IPresenter
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