using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChartATask.Core.Models;
using ChartATask.Core.Models.Acceptor;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Models.Events.AppEvents;

namespace ChartATask.Core.Persistence
{
    public class CsvPersistence : IPersistence
    {
        public void Save(List<IDataSet> dataSets)
        {
            using (var streamWriter = new StreamWriter(@"./data.csv"))
            {
                foreach (var dataSet in dataSets.Select(dataSet => dataSet as DataSet<DurationOverTime>)
                    .Where(p => p != null))
                {
                    foreach (var dataSetDataPoint in dataSet.DataPoints)
                    {
                        streamWriter.WriteLine(dataSetDataPoint.ToString());
                    }
                }
            }
        }

        public List<IDataSet> Load(string fileName)
        {
            var source =
                new DurationOverTimeDataSource<AppTitleChangedEvent>(
                    new[]
                    {
                        new Trigger<AppTitleChangedEvent>(
                            new AppTitleEventSocket(
                                new StringContains("application"),
                                new StringContains("Calculator"))
                        )
                    },
                    new[]
                    {
                        new Trigger<AppTitleChangedEvent>(
                            new AppTitleEventSocket(
                                new StringContains("application"),
                                new StringContains("Calculator"))
                        )
                    });

            var dataSet = new DataSet<DurationOverTime>(source);

            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine()?.Split(',');

                    try
                    {
                        var x = DateTime.Parse(values[0]);
                        var y = TimeSpan.Parse(values[1]);

                        dataSet.Add(new DurationOverTime(x, y));
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            return new List<IDataSet> {dataSet};
        }

        public void Dispose()
        {
        }
    }
}