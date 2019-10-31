using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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

namespace WindowsWPF
{
    public class MainViewModel : Observable
    {
        private List<SessionDuration> _dataPoints;
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
                if (_startStopContent != value)
                {
                    SetValue(ref _startStopContent, value);
                }
            }
        }

        public ICommand StartStopCommand
        {
            get
            {
                return _startStopCommand ?? (_startStopCommand = new DelegateCommand<object>(args =>
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
            }
        }


        private void Start()
        {
            _engine = new Engine(
                new CsvPersistence("./"),
                new EventWatcherManager(new IEventWatcher[]
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
            var tempModel = new PlotModel
            {
                Title = "Session duration",
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                PlotMargins = new OxyThickness(50, 0, 0, 40)
            };

            tempModel.Axes.Add(new TimeSpanAxis {StringFormat = "mm"});
            tempModel.Axes.Add(new DateTimeAxis {Position = AxisPosition.Bottom, StringFormat = "ddd hhtt"});

            foreach (var dataSet in _engine.GetDataSets().OfType<DataSet<SessionDuration>>())
            {
                dataSet.OnNewDataPoint += DataSet_OnNewDataPoint;
                _dataPoints = GetDataPoints(dataSet).ToList();

                tempModel.Series.Add(new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 2,
                    ItemsSource = _dataPoints,
                    DataFieldX = "X",
                    DataFieldY = "Y",
                    MarkerStroke = OxyColors.ForestGreen,
                    MarkerType = MarkerType.Circle,
                    InterpolationAlgorithm = new CanonicalSpline(0.3),
                    CanTrackerInterpolatePoints = true
                });
            }

            Model = tempModel;
        }

        private void DataSet_OnNewDataPoint(object sender, SessionDuration e)
        {
            var lastDataPoint = _dataPoints.Last();
            if (e.X - lastDataPoint.X >= TimeSpan.FromHours(1))
            {
                _dataPoints.Add(new SessionDuration(new DateTime(e.X.Year, e.X.Month, e.X.Day, e.X.Hour, 0, 0), e.Y));
            }
            else
            {
                _dataPoints[_dataPoints.Count - 1] = new SessionDuration(lastDataPoint.X, lastDataPoint.Y.Add(e.Y));
            }

            _model.InvalidatePlot(true);
        }

        private static IEnumerable<SessionDuration> GetDataPoints(DataSet<SessionDuration> dataSet)
        {
            var data = new Collection<SessionDuration>();

            var dataPoints = dataSet.DataPoints
                .GroupBy(dataPoint => new {dataPoint.X.Date, dataPoint.X.Hour})
                .Select(pointGroup => pointGroup
                    .Aggregate((first, second) => new SessionDuration(first.X, first.Y.Add(second.Y))))
                .Select(dataPoint =>
                    new SessionDuration(
                        new DateTime(dataPoint.X.Year, dataPoint.X.Month, dataPoint.X.Day, dataPoint.X.Hour, 0, 0),
                        dataPoint.Y))
                .ToList();

            var lastPoint = dataPoints.FirstOrDefault();

            if (lastPoint == null)
            {
                return data;
            }

            data.Add(lastPoint);
            for (var i = 1; i < dataPoints.Count; i++)
            {
                var nextPoint = dataPoints.ElementAt(i);
                var currentDataPointDate = lastPoint.X.AddHours(1);
                while (currentDataPointDate < nextPoint.X)
                {
                    data.Add(new SessionDuration(currentDataPointDate, TimeSpan.Zero));
                    currentDataPointDate = currentDataPointDate.AddHours(1);
                }

                lastPoint = nextPoint;
                data.Add(lastPoint);
            }

            return data;
        }
    }
}