using System;
using System.Collections.Generic;
using System.Diagnostics;
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

/// <summary>
/// ScottPlotのサンプル
/// 参考サイト
/// https://scottplot.net/faq/live-data/
/// </summary>
namespace Graph02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly double[] Values = new double[500];
        readonly Stopwatch Stopwatch = Stopwatch.StartNew();
        readonly ScottPlot.Plottable.VLine VerticalLine;
        int NextIndex = 0;

        /// <summary>
        /// タイマ
        /// </summary>
        private DispatcherTimer timer1 = new DispatcherTimer(DispatcherPriority.Render);
        private DispatcherTimer timer2 = new DispatcherTimer(DispatcherPriority.Render);


        public MainWindow()
        {
            InitializeComponent();

            wpfPlot.Plot.AddSignal(Values);
            VerticalLine = wpfPlot.Plot.AddVerticalLine(0, System.Drawing.Color.Red, 2);
            wpfPlot.Plot.SetAxisLimits(0, Values.Length, -2, 2);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            timer1.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer1.Tick += (e, s) => {
                AddDataPoint();
                //wpfPlot.Render();
            };

            timer2.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer2.Tick += (e, s) =>
            {
                wpfPlot.Render();
            };

            this.Closing += (e, s) => {
                timer1.Stop();
                timer2.Stop();
            };

            timer1.Start();
            timer2.Start();
        }

        public void AddDataPoint()
        {
            Values[NextIndex] = Math.Sin(Stopwatch.Elapsed.TotalSeconds * 3);

            NextIndex += 1;
            if (NextIndex >= Values.Length)
                NextIndex = 0;

            VerticalLine.X = NextIndex;
        }
    }
}
