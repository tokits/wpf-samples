using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Graph01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// タイマ
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);
        /// <summary>
        /// 表示する図形オブジェクトの最大個数
        /// </summary>
        public int MaxCount { get; private set; }

        ScottPlot.Plottable.ScatterPlot plot;
        /// <summary>
        /// 
        /// </summary>
        Queue<Tuple<double, double>> queue = new Queue<Tuple<double, double>>();
        double lastX=0.0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            MaxCount = 51;

            //描画するデータの作成
            double[] xs1 = DataGen.Consecutive(MaxCount);
            double[] ys1 = DataGen.Zeros(MaxCount);

            //chart.plt.PlotScatter(xs, sin, label: "sin");
            ////_ = chart.plt.PlotScatter(xs, cos, label: "cos");
            ////_ = chart.plt.Legend();

            //plot = plt.PlotScatter(xs, ys, label: "cos");
            plot = chart.plt.PlotScatter(xs1, ys1, label: "cos");
            //plot = chart.plt.PlotSignalXY(.PlotScatter(xs1, ys1, label: "cos");
            //chart.plt.Legend();
            //plot = chart.plt.PlotScatter(null, null, label: "cos");

            chart.plt.Title("Scatter Plot Quickstart");
            chart.plt.YLabel("Vertical Units");
            chart.plt.XLabel("Horizontal Units");
            chart.Refresh();


            ////int pointCount = 51;
            //double[] xs = DataGen.Consecutive(MaxCount);
            //double[] sin = DataGen.Sin(MaxCount);
            //chart.plt.PlotScatter(xs, sin, label: "sin");
            //chart.plt.Title("Scatter Plot Quickstart");
            //chart.plt.YLabel("Vertical Units");
            //chart.plt.XLabel("Horizontal Units");
            //chart.Refresh();


            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            timer.Tick += (e, s) => {

                var item = new Tuple<double, double>(lastX, Math.Sin(lastX * Math.PI * 0.01));
                queue.Enqueue(item);
                lastX += 1.0;

                while (queue.Count > MaxCount)
                {
                    queue.Dequeue();
                }
                double[] xs = queue.Select(x => { return x.Item1; }).ToArray();
                double[] ys = queue.Select(x => { return x.Item2; }).ToArray();
                chart.plt.Clear();
                //plot.set
                //for (var i = 0; i < /*plot.Xs.Length*/MaxCount; i++)
                //{
                //    plot.Xs[i] = xs[i];
                //    plot.Ys[i] = ys[i];
                //}
                //chart.plt.PlotSignal(ys, xs, label: "cos");

                chart.plt.PlotScatter(xs, ys, label: "cos");
                //plot.Cle
               // chart.plt.Legend();
                chart.Refresh();
                //this.InvalidateVisual();
            };
            this.Closing += (e, s) => { timer.Stop(); };

            timer.Start();
        }
    }
}
