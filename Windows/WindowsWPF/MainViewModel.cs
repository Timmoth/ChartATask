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
        private PlotModel _model;
        private Engine _engine;
        private bool isRunning;
        private string _StartStopContent;

        public MainViewModel()
        {
            isRunning = false;
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
            get => _StartStopContent;

            set
            {
                if (_StartStopContent != value)
                {
                    SetValue(ref _StartStopContent, value);
                }
            }
        }

        private ICommand _StartStopCommand;
        public ICommand StartStopCommand
        {
            get
            {
                return _StartStopCommand ?? (_StartStopCommand = new DelegateCommand<object>((args) =>
                {
                    if (isRunning)
                    {
                        _engine?.Dispose();
                        StartStopContent = "Start";
                    }
                    else
                    {
                        Start();
                        StartStopContent = "Stop";
                    }

                    isRunning = !isRunning;
                }));
            }
        }


        private void Start()
        {
            var eventCollector = new EventWatcherManager();
            eventCollector.Register(new WindowsKeyboardEventWatcher());
            eventCollector.Register(new WindowsRunningAppEventWatcher());
            eventCollector.Register(new WindowsAppTitleEventWatcher());
            eventCollector.Register(new WindowsFocusedAppEventWatcher());

            var requestManager = new RequestEvaluatorManager();
            requestManager.Register(new WindowsAppRunningRequest());

            _engine = new Engine(
                new CsvPersistence("./"),
                eventCollector,
                requestManager);

            CreateChart();
        }

        private void CreateChart()
        {
            var tmp = new PlotModel
            {
                Title = "Session duration",
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                PlotMargins = new OxyThickness(50, 0, 0, 40)
            };

            tmp.Axes.Add(new TimeSpanAxis {StringFormat = "mm"});
            tmp.Axes.Add(new DateTimeAxis {Position = AxisPosition.Bottom, StringFormat = "ddd hhtt"});

            foreach (var dataSet in _engine.GetDataSets().OfType<DataSet<SessionDuration>>())
            {
                tmp.Series.Add(new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 2,
                    ItemsSource = GetDataPoints(dataSet),
                    DataFieldX = "X",
                    DataFieldY = "Y",
                    MarkerStroke = OxyColors.ForestGreen,
                    MarkerType = MarkerType.Circle,
                    InterpolationAlgorithm = new CanonicalSpline(0.3),
                    CanTrackerInterpolatePoints = true
                });
            }

            Model = tmp;
        }

        private static IEnumerable<SessionDuration> GetDataPoints(DataSet<SessionDuration> dataSet)
        {
            var data = new Collection<SessionDuration>();

            var dataPoints = dataSet.DataPoints
                .GroupBy(q => new {q.X.Date, q.X.Hour})
                .Select(pointGroup => pointGroup
                    .Aggregate((o1, o2) => new SessionDuration(o1.X, o1.Y.Add(o2.Y))))
                .Select(o => new SessionDuration(new DateTime(o.X.Year, o.X.Month, o.X.Day, o.X.Hour, 0, 0), o.Y))
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