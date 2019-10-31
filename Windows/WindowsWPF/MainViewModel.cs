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
            var dataLoader = new CsvPersistence("./");

            var dataSet = dataLoader.Load().ElementAt(0) as DataSet<SessionDuration>;

            var tmp = new PlotModel
            {
                Title = "Session duration",
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                PlotMargins = new OxyThickness(50, 0, 0, 40)
            };
            tmp.Axes.Add(new TimeSpanAxis {StringFormat = "h:mm"});
            tmp.Axes.Add(new DateTimeAxis {Position = AxisPosition.Bottom});

            var data = new Collection<SessionDuration>();

            dataSet?.DataPoints
                .GroupBy(q => new
                {
                    q.X.Date,
                    q.X.Hour,
                    q.X.Minute
                }).Select(pointGroup =>
                    pointGroup.Aggregate((o1, o2) => new SessionDuration(o1.X, o1.Y.Add(o2.Y)))
                ).ToList().ForEach(point => data.Add(point));


            tmp.Series.Add(new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 2,
                ItemsSource = data,
                DataFieldX = "X",
                DataFieldY = "Y",
                MarkerStroke = OxyColors.ForestGreen,
                MarkerType = MarkerType.Circle,
                InterpolationAlgorithm = new CanonicalSpline(0.05),
                CanTrackerInterpolatePoints = false
            });

            Model = tmp;
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
    }
}