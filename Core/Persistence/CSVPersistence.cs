using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChartATask.Core.Events.AppEvents;
using ChartATask.Core.Models;
using ChartATask.Core.Models.Acceptor;
using ChartATask.Core.Models.Conditions;
using ChartATask.Core.Models.DataPoints;

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
                new DurationOverTimeDataSource(
                    new[]
                    {
                        new Trigger(
                            new AppTitleSocket(
                                new RegularExpressionAcceptor("firefox"),
                                new RegularExpressionAcceptor("GitHub")),
                            new SystemTimeCondition(new Always<DateTime>(true))
                        )
                    },
                    new[]
                    {
                        new Trigger(
                            new AppTitleSocket(
                                new RegularExpressionAcceptor("firefox"),
                                new NotAcceptor<string>(new RegularExpressionAcceptor("GitHub"))),
                            new SystemTimeCondition(new Always<DateTime>(true))
                        )
                    });

            var fireFoxTabSwitchDataSet = LoadDataSet($@"{directory}0.csv", firefoxTitleChangeDataSource);

            return new List<IDataSet> {fireFoxTabSwitchDataSet};
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

        public void Dispose()
        {
        }
    }
}