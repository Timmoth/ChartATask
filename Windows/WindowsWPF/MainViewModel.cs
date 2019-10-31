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
                Title = "Test",
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                PlotMargins = new OxyThickness(50, 0, 0, 40)
            };

            var ls = new LineSeries {Title = "Header"};
            if (dataSet != null)
            {
                foreach (var item in dataSet.DataPoints)
                {
                    var x = item.X.Ticks;
                    var y = item.Y.TotalSeconds;
                    ls.Points.Add(new DataPoint(x, y));
                }
            }

            tmp.Series.Add(ls);

            tmp.Axes.Add(new LinearAxis {Position = AxisPosition.Bottom, Title = "TestHeader"});
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