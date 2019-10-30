using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChartATask.Core.Models;
using ChartATask.Core.Models.Acceptor;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Models.Events.AppEvents;

namespace ChartATask.Core.Persistence
{
    public class CsvPersistence : IPersistence
    {
        public void Save(List<IDataSet> dataSets)
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

        public List<IDataSet> Load(string directory)
        {
            var firefoxTitleChangeDataSource =
                new DurationOverTimeDataSource<AppTitleChanged>(
                    new[]
                    {
                        new Trigger<AppTitleChanged>(
                            new AppTitleSocket(
                                new RegularExpressionAcceptor("firefox"),
                                new RegularExpressionAcceptor("GitHub"))
                        )
                    },
                    new[]
                    {
                        new Trigger<AppTitleChanged>(
                            new AppTitleSocket(
                                new RegularExpressionAcceptor("firefox"),
                                new NotAcceptor<string>( new RegularExpressionAcceptor("GitHub")))
                        )
                    });

            var calculatorFocusDataSource =
                new DurationOverTimeDataSource<AppFocusChanged>(
                    new[]
                    {
                        new Trigger<AppFocusChanged>(
                            new AppFocusSocket(
                                new RegularExpressionAcceptor("firefox"),
                                new Always(true))
                        )
                    },
                    new[]
                    {
                        new Trigger<AppFocusChanged>(
                            new AppFocusSocket(
                                new NotAcceptor<string>( new RegularExpressionAcceptor("firefox")),
                                new Always(true))
                        )
                    });

            var fireFoxTabSwitchDataSet = LoadDataSet($@"{directory}0.csv", firefoxTitleChangeDataSource);
            var calculatorFocusDataSet = LoadDataSet($@"{directory}1.csv", calculatorFocusDataSource);

            return new List<IDataSet> {fireFoxTabSwitchDataSet, calculatorFocusDataSet};
        }

        private static DataSet<DurationOverTime> LoadDataSet<TEvent>(
            string fileName,
            DurationOverTimeDataSource<TEvent> dataSource) where TEvent : IEvent
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

        public void Dispose()
        {
        }
    }
}