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
using static System.IO.File;

namespace ChartATask.Core.Persistence
{
    public class CsvPersistence : IPersistence
    {
        private readonly string _directory;

        public CsvPersistence(string directory)
        {
            _directory = directory;
        }

        public IEnumerable<IDataSet> Load()
        {
            var dataSources = new List<SessionDurationSource>()
            {
                new SessionDurationSource(),
            //    new FilteredSessionDurationSource(
            //        new[]
            //        {
            //            new Trigger(
            //                new AppFocusSocket(
            //                    new RegularExpressionAcceptor("devenv"),
            //                    new RegularExpressionAcceptor("Experiment")),
            //                new SystemTimeCondition(new Always<DateTime>(true))
            //            )
            //        },
            //        new[]
            //        {
            //            new Trigger(
            //                new AppFocusSocket(
            //                    new Always<string>(true),
            //                    new NotAcceptor<string>(new RegularExpressionAcceptor("Experiment"))),
            //                new SystemTimeCondition(new Always<DateTime>(true))
            //            )
            //        }),
            //        new FilteredSessionDurationSource(
            //        new[]
            //        {
            //            new Trigger(
            //                new AppFocusSocket(
            //                    new RegularExpressionAcceptor("devenv"),
            //                    new RegularExpressionAcceptor("DMV")),
            //                new SystemTimeCondition(new Always<DateTime>(true))
            //            )
            //        },
            //        new[]
            //        {
            //            new Trigger(
            //                new AppFocusSocket(
            //                    new Always<string>(true),
            //                    new NotAcceptor<string>(new RegularExpressionAcceptor("DMV"))),
            //                new SystemTimeCondition(new Always<DateTime>(true))
            //            )
            //        }),
            //new FilteredSessionDurationSource(
            //        new[]
            //        {
            //            new Trigger(
            //                new AppFocusSocket(
            //                    new RegularExpressionAcceptor("devenv"),
            //                    new RegularExpressionAcceptor("chartatask")),
            //                new SystemTimeCondition(new Always<DateTime>(true))
            //            )
            //        },
            //        new[]
            //        {
            //            new Trigger(
            //                new AppFocusSocket(
            //                    new Always<string>(true),
            //                    new NotAcceptor<string>(new RegularExpressionAcceptor("chartatask"))),
            //                new SystemTimeCondition(new Always<DateTime>(true))
            //            )
            //        }),
            };

            var dataSets = new List<IDataSet>();

            for(var i = 0; i < dataSources.Count; i++)
            {
                try
                {
                    dataSets.Add(LoadDataSet($@"{_directory}{i}.csv", dataSources[i]));
                }
                catch
                {
                    //Could not load data set
                }
            }

            return dataSets;
        }

        public void Dispose()
        {
        }

        private static DataSet<SessionDuration> LoadDataSet(
            string fileName,
            SessionDurationSource dataSource)
        {
            var dataSet = new DataSet<SessionDuration>(dataSource);

            using (var reader = GetStreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine()?.Split(',');

                    try
                    {
                        var name = values[0];
                        var title = values[1];   
                        var x = DateTime.Parse(values[2]);
                        var y = DateTime.Parse(values[3]);

                        dataSet.Add(new SessionDuration(name, title,x, y));
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            return dataSet;
        }
        public void Save(IEnumerable<IDataSet> dataSets)
        {
            var durationOverTimeDataSets = dataSets
                .Select(dataSet => dataSet as DataSet<SessionDuration>)
                .Where(p => p != null)
                .ToList();

            for (var i = 0; i < durationOverTimeDataSets.Count(); i++)
            {
                try
                {
                    var dataSet = durationOverTimeDataSets[i];
                    using (var streamWriter = GetStreamWriter($@"./{i}.csv"))
                    {
                        foreach (var dataSetDataPoint in dataSet.DataPoints)
                        {
                            streamWriter.WriteLine(dataSetDataPoint.ToString());
                        }
                    }
                }catch
                {
                    //Could not save data set
                }
               
            }
        }


        private static StreamReader GetStreamReader(string filename)
        {
            return Exists(filename) ? new StreamReader(filename) : new StreamReader(Create(filename));
        }       
        
        private static StreamWriter GetStreamWriter(string filename)
        {
            return Exists(filename) ? new StreamWriter(filename) : new StreamWriter(Create(filename));
        }
    }
}