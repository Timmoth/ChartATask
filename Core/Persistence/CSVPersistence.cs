using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChartATask.Core.Acceptor;
using ChartATask.Core.Conditions;
using ChartATask.Core.DataPoints;
using ChartATask.Core.Events.Sockets;

namespace ChartATask.Core.Persistence
{
    public class CsvPersistence : IPersistence
    {
        private readonly string _directory;

        public CsvPersistence(string directory)
        {
            _directory = directory;
        }

        public void Save(IEnumerable<IDataSet> dataSets)
        {
            var durationOverTimeDataSets = dataSets.Select(dataSet => dataSet as DataSet<DurationOverTime>)
                .Where(p => p != null).ToList();
            for (var i = 0; i < durationOverTimeDataSets.Count(); i++)
            {
                var dataSet = durationOverTimeDataSets[i];
                using (var streamWriter = new StreamWriter($@"./{i}.csv"))
                {
                    foreach (var dataSetDataPoint in dataSet.DataPoints)
                    {
                        streamWriter.WriteLine(dataSetDataPoint.ToString());
                    }
                }
            }
        }

        public IEnumerable<IDataSet> Load()
        {
            var firefoxTitleChangeDataSource =
                new DurationOverTimeDataSource(
                    new[]
                    {
                        new Trigger(
                            new AppFocusSocket(
                                new RegularExpressionAcceptor("devenv"),
                                new RegularExpressionAcceptor("DMV")),
                            new SystemTimeCondition(new Always<DateTime>(true))
                        )
                    },
                    new[]
                    {
                        new Trigger(
                            new AppFocusSocket(
                                new Always<string>(true), 
                                new NotAcceptor<string>(new RegularExpressionAcceptor("DMV"))),
                            new SystemTimeCondition(new Always<DateTime>(true))
                        )
                    });

            var fireFoxTabSwitchDataSet = LoadDataSet($@"{_directory}0.csv", firefoxTitleChangeDataSource);

            return new List<IDataSet> {fireFoxTabSwitchDataSet};
        }

        public void Dispose()
        {
        }

        private static DataSet<DurationOverTime> LoadDataSet(
            string fileName,
            DurationOverTimeDataSource dataSource)
        {
            var dataSet = new DataSet<DurationOverTime>(dataSource);

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

            return dataSet;
        }
    }
}