using System;
using System.IO;
using ChartATask.Core.Models;
using ChartATask.Core.Models.DataPoints;

namespace ChartATask.Core.Persistence
{
    public class CsvPersistence : IPersistence<DurationOverTime>
    {
        public void Dispose()
        {
        }

        public void Save(DataSet<DurationOverTime> dataSet)
        {
            using (var streamWriter = new StreamWriter(@"./data.csv"))
            {
                if (dataSet == null)
                {
                    return;
                }

                foreach (var dataSetDataPoint in dataSet.DataPoints)
                {
                    streamWriter.WriteLine(dataSetDataPoint.ToString());
                }
            }
        }

        public DataSet<DurationOverTime> Load(string fileName)
        {
            var dataSet = new DataSet<DurationOverTime>();

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