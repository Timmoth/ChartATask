using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChartATask.Core.Data;
using ChartATask.Core.Data.Points;
using ChartATask.Core.Persistence;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace WindowsWPF
{
    public class MainViewModel : Observable
    {
        private PlotModel _model;

        public MainViewModel()
        {
            var csvPersistence = new CsvPersistence("./");
            CreateChart(csvPersistence.Load());
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


        private void CreateChart(IEnumerable<IDataSet> dataCollection)
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

            foreach (var dataSet in dataCollection.OfType<DataSet<SessionDuration>>())
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