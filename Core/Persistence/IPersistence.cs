using System.Collections.Generic;
using ChartATask.Core.Models;

namespace ChartATask.Core.Persistence
{
    public interface IPersistence
    {
        void Save(List<IDataSet> dataSet);
        List<IDataSet> Load(string directory);
    }
}