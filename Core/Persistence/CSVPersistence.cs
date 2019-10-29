using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChartATask.Core.Models;
using ChartATask.Core.Models.DataPoints;

namespace ChartATask.Core.Persistence
{
    public class CSVPersistence : IPersistence
    {
        public void Dispose()
        {
        }

        public void Save(DataSetCollection dataSetCollection)
        {
            using (var streamWriter = new StreamWriter(@"./data.csv"))
            {
                var dataSet = dataSetCollection.DataSets.ElementAt(0) as AppSessionDataSet;

                if (dataSet == null)
                {
                    return;
                }

                streamWriter.WriteLine(dataSet.AppName);
                streamWriter.WriteLine(dataSet.AppTitle);
                foreach (var dataSetDataPoint in dataSet.DataPoints)
                {
                    streamWriter.WriteLine(dataSetDataPoint.ToString());
                }
            }
        }

        public DataSetCollection Load()
        {
            var dataSets = new List<IDataSet>();
            using (var reader = new StreamReader(@"./data.csv"))
            {
                var appName = reader.ReadLine();
                var appTitle = reader.ReadLine();
                var dataSet = new AppSessionDataSet(appName, appTitle);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var x = DateTime.Parse(values[0]);
                    var y = TimeSpan.Parse(values[1]);

                    dataSet.Add(new DurationOverTime(x, y));
                }

                dataSets.Add(dataSet);
            }

            return new DataSetCollection(dataSets);
        }
    }
}