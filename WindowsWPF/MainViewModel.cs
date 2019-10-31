using ChartATask.Core.Models;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Persistence;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Linq;

namespace WindowsWPF
{
    public class MainViewModel : Observable
    {
        private PlotModel _model;

        public PlotModel Model
        {
            get => this._model;

            set
            {
                if (this._model != value)
                {
                    this.SetValue(ref this._model, value);
                }
            }
        }


        public MainViewModel()
        {
            var dataLoader = new CsvPersistence("./");

            var dataSet = dataLoader.Load().ElementAt(0) as DataSet<DurationOverTime>;

            var tmp = new PlotModel
            {
                Title = "Test",
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                PlotMargins = new OxyThickness(50, 0, 0, 40)
            };

            var ls = new LineSeries { Title = "Header" };
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

            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "TestHeader" });
            this.Model = tmp;

        }
    }
}
