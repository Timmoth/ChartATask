using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChartATask.Core.Data;
using ChartATask.Core.Data.Points;
using ChartATask.Core.Data.Sources;
using ChartATask.Core.Triggers;
using ChartATask.Core.Triggers.Acceptor;
using ChartATask.Core.Triggers.Conditions;
using ChartATask.Core.Triggers.Events.Sockets;

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
            var durationOverTimeDataSets = dataSets.Select(dataSet => dataSet as DataSet<SessionDuration>)
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
                new SessionDurationSource(
                    new[]
                    {
                        new Trigger(
                            new AppFocusSocket(
                                new RegularExpressionAcceptor("devenv"),
                                new RegularExpressionAcceptor("Experiment")),
                            new SystemTimeCondition(new Always<DateTime>(true))
                        )
                    },
                    new[]
                    {
                        new Trigger(
                            new AppFocusSocket(
                                new Always<string>(true),
                                new NotAcceptor<string>(new RegularExpressionAcceptor("Experiment"))),
                            new SystemTimeCondition(new Always<DateTime>(true))
                        )
                    });

            var dataSet = LoadDataSet($@"{_directory}0.csv", firefoxTitleChangeDataSource);

            return new List<IDataSet> { dataSet };
        }

        public void Dispose()
        {
        }

        private static DataSet<SessionDuration> LoadDataSet(
            string fileName,
            SessionDurationSource dataSource)
        {
            var dataSet = new DataSet<SessionDuration>(dataSource);

            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine()?.Split(',');

                    try
                    {
                        var x = DateTime.Parse(values[0]);
                        var y = DateTime.Parse(values[1]);

                        dataSet.Add(new SessionDuration(x, y));
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