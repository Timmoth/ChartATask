using ChartATask.Core;
using ChartATask.Core.Data;
using ChartATask.Core.Data.Points;
using ChartATask.Core.Persistence;
using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Requests;
using ChartATask.Interactors.Windows.Requests;
using ChartATask.Interactors.Windows.Watchers;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace WindowsWPF
{
    public class MainViewModel : Observable
    {
        private List<List<DoubleOverTime>> _dataSetList;
        private Engine _engine;
        private bool _isRunning;
        private PlotModel _model;
        private ICommand _startStopCommand;
        private string _startStopContent;

        public MainViewModel()
        {
            _isRunning = false;
            StartStopContent = "Start";
        }

        public PlotModel Model
        {
            get => _model;

            set
            {
                if (_model != value)
                {
                    SetValue(ref _model, value);
                }
            }
        }

        public string StartStopContent
        {
            get => _startStopContent;

            set
            {
                if (string.Compare(_startStopContent, value) != 0)
                {
                    SetValue(ref _startStopContent, value);
                }
            }
        }

        public ICommand StartStopCommand => _startStopCommand ?? (_startStopCommand = new DelegateCommand<object>(args =>
                                                          {
                                                              if (_isRunning)
                                                              {
                                                                  _engine?.Dispose();
                                                                  StartStopContent = "Start";
                                                              }
                                                              else
                                                              {
                                                                  Start();
                                                                  StartStopContent = "Stop";
                                                              }

                                                              _isRunning = !_isRunning;
                                                          }));


        private void Start()
        {
            _engine = new Engine(
                new CsvPersistence("./"),
                new EventWatcherManager(new EventWatcher[]
                {
                    new WindowsKeyboardEventWatcher(),
                    new WindowsRunningAppEventWatcher(),
                    new WindowsAppTitleEventWatcher(),
                    new WindowsFocusedAppEventWatcher()
                }),
                new RequestEvaluatorManager(new[]
                {
                    new WindowsAppRunningRequest()
                }));

            CreateChart();
        }

        private void CreateChart()
        {
            _dataSetList = new List<List<DoubleOverTime>>();

            var tempModel = new PlotModel
            {
                Title = @"Task Activity",
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                PlotMargins = new OxyThickness(10, 10, 10, 50),
                PlotType = PlotType.XY
            };

            var r = new Random();

            tempModel.Axes.Add(new LinearAxis { AxisDistance = 5, IsAxisVisible = false });
            tempModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "ddd hhtt", Title = "Date / Time", AxisDistance = 5 });

            foreach (var dataSet in _engine.GetDataSets().OfType<DataSet<SessionDuration>>())
            {
                foreach (var groupedDataSets in GetDataSets(dataSet).ToList())
                {
                    //dataSet.OnNewDataPoint += (s, e) =>
                    //{
                    //    if (newDataSet.Count == 0)
                    //    {
                    //        return;
                    //    }

                    //    var lastDataPoint = newDataSet.Last();
                    //    if (e.X - lastDataPoint.X >= TimeSpan.FromHours(1))
                    //    {
                    //        newDataSet.Add(new DoubleOverTime(new DateTime(e.X.Year, e.X.Month, e.X.Day, e.X.Hour, 0, 0),
                    //            (e.Y - e.X).TotalMinutes * 1.666));
                    //    }
                    //    else
                    //    {
                    //        newDataSet[newDataSet.Count - 1] = new DoubleOverTime(lastDataPoint.X,
                    //            lastDataPoint.Y + (e.Y - e.X).TotalMinutes * 1.666);
                    //    }

                    //    _model.InvalidatePlot(true);
                    //};

                    var randomBytes = new byte[3];
                    r.NextBytes(randomBytes);

                    tempModel.Series.Add(new LineSeries
                    {
                        StrokeThickness = 2,
                        MarkerSize = 2,
                        ItemsSource = groupedDataSets.Item3.ToList(),
                        Title = $"{groupedDataSets.Item1} {groupedDataSets.Item2}",
                        DataFieldX = "X",
                        DataFieldY = "Y",
                        Color = OxyColor.FromRgb(randomBytes[0], randomBytes[1], randomBytes[2]),
                        MarkerStroke = OxyColor.FromRgb(randomBytes[0], randomBytes[1], randomBytes[2]),
                        MarkerType = MarkerType.Circle,
                        InterpolationAlgorithm = new CanonicalSpline(0.1),
                        CanTrackerInterpolatePoints = true,

                    });
                    _dataSetList.Add(groupedDataSets.Item3.ToList());
                }
            }

            Model = tempModel;
        }

        public class DoubleOverTime
        {
            public DateTime X { get; set; }
            public double Y { get; set; }

            public DoubleOverTime(DateTime x, double y)
            {
                X = x;
                Y = y;
            }
        }

        private IEnumerable<(string, string, IEnumerable<DoubleOverTime>)> GetDataSets(DataSet<SessionDuration> dataSet) => dataSet.DataPoints
                .GroupBy(point => (point.Name, point.Title))
                .Select(groupedDataPoints =>
                    (groupedDataPoints.Key.Name,
                        groupedDataPoints.Key.Title,
                            GetDataPoints(groupedDataPoints)));

        private static IEnumerable<DoubleOverTime> GetDataPoints(IEnumerable<SessionDuration> dataPoints)
        {
            var data = new Collection<DoubleOverTime>();

            var hourlyPercentagePoints = dataPoints
                .GroupBy(dataPoint => new { dataPoint.X.Date, dataPoint.X.Hour })
                .Select(pointGroup => pointGroup
                    .Aggregate((first, second) => new SessionDuration(first.Name, first.Title, first.X, first.X + ((first.Y - first.X) + (second.Y - second.X)))))
                .Select(dataPoint =>
                    new DoubleOverTime(
                        new DateTime(dataPoint.X.Year, dataPoint.X.Month, dataPoint.X.Day, dataPoint.X.Hour, 0, 0),
                        (dataPoint.Y - dataPoint.X).TotalMinutes * 1.666))
                .ToList();



            var lastPoint = hourlyPercentagePoints.FirstOrDefault();

            if (lastPoint == null)
            {
                return data;
            }

            data.Add(lastPoint);
            for (var i = 1; i < hourlyPercentagePoints.Count; i++)
            {
                var nextPoint = hourlyPercentagePoints.ElementAt(i);
                var currentDataPointDate = lastPoint.X.AddHours(1);
                while (currentDataPointDate < nextPoint.X)
                {
                    data.Add(new DoubleOverTime(currentDataPointDate, 0));
                    currentDataPointDate = currentDataPointDate.AddHours(1);
                }

                lastPoint = nextPoint;
                data.Add(lastPoint);
            }

            return data;
        }
    }
}