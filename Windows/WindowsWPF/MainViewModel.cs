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
        private List<DoubleOverTime> _dataPoints;
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

            //var model = new PlotModel { Title = "Task heatmap" };

            //// Weekday axis (horizontal)
            //model.Axes.Add(new CategoryAxis
            //{
            //    Position = AxisPosition.Bottom,

            //    // Key used for specifying this axis in the HeatMapSeries
            //    Key = "Hour",

            //    // Array of Categories (see above), mapped to one of the coordinates of the 2D-data array
            //    ItemsSource = new[]
            //    {
            //        "00",
            //        "01",
            //        "02",
            //        "03",
            //        "04",
            //        "05",
            //        "06",
            //        "07",
            //        "08",
            //        "09",
            //        "10",
            //        "11",
            //        "12",
            //        "13",
            //        "14",
            //        "15",
            //        "16",
            //        "17",
            //        "18",
            //        "19",
            //        "20",
            //        "21",
            //        "22",
            //        "23",
            //    }
            //});

            //// Cake type axis (vertical)
            //model.Axes.Add(new CategoryAxis
            //{
            //    Position = AxisPosition.Left,
            //    Key = "Task",
            //    ItemsSource = new[]
            //    {
            //        "ChartATask",
            //        "TaskPlex",
            //        "StateNet",
            //    }
            //});

            //// Color axis
            //model.Axes.Add(new LinearColorAxis
            //{
            //    Palette = new OxyPalette(new []
            //    {
            //        OxyColor.FromRgb(  0,107,179), 
            //        OxyColor.FromRgb(    0,122,204), 
            //        OxyColor.FromRgb(       0,138,230), 
            //        OxyColor.FromRgb(       0,153,255), 
            //        OxyColor.FromRgb(      26,163,255), 
            //        OxyColor.FromRgb(       51,173,255), 
            //        OxyColor.FromRgb(       77,184,255),
            //        })
            //});

            //var rand = new Random();
            //var data = new double[18, 3];


            //var datapoints = GetDataPoints(_engine.GetDataSets().OfType<DataSet<SessionDuration>>().First()).Select(o => (int)o.Y);

            //for (int x = 0; x < 3; x++)
            //{
            //    for (int y = 0; y < 18; y++)
            //    {
            //        data[y, x] = datapoints.ElementAt((x * y) % 18);
            //    }
            //}


            //var heatMapSeries = new HeatMapSeries
            //{
            //    X0 = 0,
            //    X1 = 24,
            //    Y0 = 0,
            //    Y1 = 2,
            //    XAxisKey = "Hour",
            //    YAxisKey = "Task",
            //    RenderMethod = HeatMapRenderMethod.Bitmap,
            //    LabelFontSize = 0.1, // neccessary to display the label
            //    Data = data,
            //    LabelFormatString = "#"
            //};

            //model.Series.Add(heatMapSeries);
            //Model = model;
            //return;



            var tempModel = new PlotModel
            {
                Title = @"Task Activity",
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                PlotMargins = new OxyThickness(10,10,10,50),
                PlotType = PlotType.XY
            };

            tempModel.Axes.Add(new LinearAxis {AxisDistance = 5, IsAxisVisible = false });
            tempModel.Axes.Add(new DateTimeAxis {Position = AxisPosition.Bottom, StringFormat = "ddd hhtt", Title = "Date / Time", AxisDistance = 5 });

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
                    CanTrackerInterpolatePoints = true,
                    
                });
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

        private void DataSet_OnNewDataPoint(object sender, SessionDuration e)
        {
            var lastDataPoint = _dataPoints.Last();
            if (e.X - lastDataPoint.X >= TimeSpan.FromHours(1))
            {
                _dataPoints.Add(new DoubleOverTime(new DateTime(e.X.Year, e.X.Month, e.X.Day, e.X.Hour, 0, 0), (e.Y - e.X).TotalMinutes * 1.666));
            }
            else
            {
                _dataPoints[_dataPoints.Count - 1] = new DoubleOverTime(lastDataPoint.X, lastDataPoint.Y + (e.Y - e.X).TotalMinutes * 1.666);
            }

            _model.InvalidatePlot(true);
        }

        private static IEnumerable<DoubleOverTime> GetDataPoints(DataSet<SessionDuration> dataSet)
        {
            var data = new Collection<DoubleOverTime>();

            var dataPoints = dataSet.DataPoints
                .GroupBy(dataPoint => new {dataPoint.X.Date, dataPoint.X.Hour})
                .Select(pointGroup => pointGroup
                    .Aggregate((first, second) => new SessionDuration(first.X, first.X + ((first.Y - first.X) + (second.Y - second.X)))))
                .Select(dataPoint =>
                    new DoubleOverTime(
                        new DateTime(dataPoint.X.Year, dataPoint.X.Month, dataPoint.X.Day, dataPoint.X.Hour, 0, 0),
                        (dataPoint.Y - dataPoint.X).TotalMinutes * 1.666))
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